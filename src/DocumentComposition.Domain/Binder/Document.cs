using System.Net.Mime;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Domain.Binder;

public sealed class Document
{
    public DocumentId Id { get; } = null!;
    public DocumentOrder Order { get; } = null!;
    public StorageUri SourceUri { get; } = null!;
    public ContentType MimeType { get; } = null!;
    public DateTime UploadedAt { get; }

    private Document(DocumentId id, DocumentOrder order, StorageUri sourceUri, ContentType mimeType)
    {
        Id = id;
        Order = order;
        SourceUri = sourceUri;
        MimeType = mimeType;
    }

    public static Result<Document> Create(DocumentId id, DocumentOrder order, StorageUri sourceUri, ContentType mimeType)
    {
        var document = new Document(id, order, sourceUri, mimeType);
        return Result.From(document);
    }
}
