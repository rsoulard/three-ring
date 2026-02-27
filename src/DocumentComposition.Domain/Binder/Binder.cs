using System.Net.Mime;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Domain.Binder;

public sealed class Binder : AggregateRoot<BinderId>
{
    public string Name { get; private set; }
    public BinderStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ExpiresAt { get; private set; }
    public StorageUri? OutputUri { get; private set; }

    private readonly List<Document> documents = [];
    public IReadOnlyList<Document> Documents => documents.AsReadOnly();

    private Binder(BinderId binderId, string name)
    {
        Id = binderId;
        Name = name;

        CreatedAt = DateTimeOffset.UtcNow;
        Status = BinderStatus.Created;
    }

    public static Result<Binder> Create(BinderId id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failed<Binder>(new ValidationError("name", "Binder's name cannot be null or empty."));
        }

        var binder = new Binder(id, name);
        binder.Raise(new BinderCreated(id, name));
        return Result.From(binder);
    }

    public Result AddDocument(DocumentId documentId, StorageUri sourceUri, ContentType mimeType)
    {
        var canDocumentsBeAddedResult = EnsureDocumentsCanBeAdded();
        if (!canDocumentsBeAddedResult.IsSuccessful)
        {
            return Result.Failed(canDocumentsBeAddedResult.Error);
        }

        if (documents.Any(document => document.Id == documentId))
        {
            return Result.Failed(new ValidationError("documentId", "A document with the same ID has already been added to the binder."));
        }

        var nextOrderResult = GetNextDocumentOrder();
        if (!nextOrderResult.IsSuccessful)
        {
            return Result.Failed(nextOrderResult.Error);
        }

        var documentResult = Document.Create(documentId, nextOrderResult.Value, sourceUri, mimeType);
        if (!documentResult.IsSuccessful)
        {
            return Result.Failed(documentResult.Error);
        }

        documents.Add(documentResult.Value);

        if (Status == BinderStatus.Created)
        {
            Status = BinderStatus.DocumentsAdded;
        }

        Raise(new DocumentAdded(Id, documentId));

        return Result.Successful();
    }

    public Result RequestComposition()
    {
        var canCompositionBeRequestedResult = EnsureCompositionCanBeRequested();
        if (!canCompositionBeRequestedResult.IsSuccessful)
        {
            return Result.Failed(canCompositionBeRequestedResult.Error);
        }
        
        Status = BinderStatus.ReadyForComposition;
        Raise(new CompositionRequested(Id));

        return Result.Successful();
    }

    private Result<DocumentOrder> GetNextDocumentOrder()
    {
        return DocumentOrder.From(documents.Count + 1);
    }

    private Result EnsureDocumentsCanBeAdded()
    {
        if (Status == BinderStatus.Expired)
        {
            return Result.Failed(new ValidationError("status", "Documents can not be added after binder is expired."));
        }

        if (Status == BinderStatus.ReadyForComposition || Status == BinderStatus.Composing)
        {
            return Result.Failed(new ValidationError("status", "Documents can not be added after composition has been requested"));
        }

        return Result.Successful();
    }

    private Result EnsureCompositionCanBeRequested()
    {
        if (Status == BinderStatus.Expired)
        {
            return Result.Failed(new ValidationError("status", "Composition can not be requested after binder is expired."));
        }

        if (Status != BinderStatus.DocumentsAdded)
        {
            return Result.Failed(new ValidationError("status", "Composition can only be requested when documents have been added and composition has not yet been requested."));
        }

        return Result.Successful();
    }
}
