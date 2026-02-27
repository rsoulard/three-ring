using System.Net.Mime;
using DocumentComposition.Domain.Binder;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Domain.Tests.Binder;

public class BinderTests
{
    [Fact]
    public void Create_ShouldCreateBinder_WhenNameIsValid()
    {
        var id = BinderId.Create();
        var name = "Test Binder";

        var result = Domain.Binder.Binder.Create(id, name);

        Assert.True(result.IsSuccessful);
        Assert.Equal(id, result.Value.Id);
        Assert.Equal(name, result.Value.Name);
        Assert.Equal(BinderStatus.Created, result.Value.Status);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldFail_WhenNameIsWhiteSpace(string whiteSpace)
    {
        var id = BinderId.Create();
        var name = whiteSpace;

        var result = Domain.Binder.Binder.Create(id, name);

        Assert.False(result.IsSuccessful);
        Assert.IsType<ValidationError>(result.Error);
    }

    [Fact]
    public void AddDocument_ShouldAddDocumentAndRaiseEvent_WhenBinderInValidState()
    {
        var id = BinderId.Create();
        var name = "Test Binder";
        var binder = Domain.Binder.Binder.Create(id, name).Value;

        var documentId = DocumentId.Create();
        var sourceUri = StorageUri.From("file://home/ryan/Downloads/document.txt").Value;
        var mimeType = new ContentType(MediaTypeNames.Text.Plain);

        var result = binder.AddDocument(documentId, sourceUri, mimeType);

        Assert.True(result.IsSuccessful);
        Assert.Equal(BinderStatus.DocumentsAdded, binder.Status);
        Assert.Single(binder.Documents, document => document.Id == documentId);
        Assert.Single(binder.DomainEvents, domainEvent => domainEvent is DocumentAdded documentAdded
            && documentAdded.Id == id
            && documentAdded.DocumentId == documentId
        );
    }

    [Fact]
    public void AddDocument_ShouldFail_WhenBinderComposing()
    {
        var id = BinderId.Create();
        var name = "Test Binder";
        var binder = Domain.Binder.Binder.Create(id, name).Value;

        var documentId1 = DocumentId.Create();
        var sourceUri1 = StorageUri.From("file://home/ryan/Downloads/document.txt").Value;
        var mimeType1 = new ContentType(MediaTypeNames.Text.Plain);

        binder.AddDocument(documentId1, sourceUri1, mimeType1);
        binder.RequestComposition();

        var documentId2 = DocumentId.Create();
        var sourceUri2 = StorageUri.From("file://home/ryan/Downloads/document.txt").Value;
        var mimeType2 = new ContentType(MediaTypeNames.Text.Plain);

        var result = binder.AddDocument(documentId2, sourceUri2, mimeType2);

        Assert.False(result.IsSuccessful);
        Assert.IsType<ValidationError>(result.Error);
        Assert.DoesNotContain(binder.Documents, document => document.Id == documentId2);
        Assert.DoesNotContain(binder.DomainEvents, domainEvent => domainEvent is DocumentAdded documentAdded
            && documentAdded.Id == id
            && documentAdded.DocumentId == documentId2
        );
    }

    [Fact]
    public void AddDocument_ShouldFail_WhenDocumentWithIdAlreadyAdded()
    {
        var id = BinderId.Create();
        var name = "Test Binder";
        var binder = Domain.Binder.Binder.Create(id, name).Value;

        var documentId = DocumentId.Create();
        var sourceUri = StorageUri.From("file://home/ryan/Downloads/document.txt").Value;
        var mimeType = new ContentType(MediaTypeNames.Text.Plain);

        binder.AddDocument(documentId, sourceUri, mimeType);
        var result = binder.AddDocument(documentId, sourceUri, mimeType);

        Assert.False(result.IsSuccessful);
        Assert.IsType<ValidationError>(result.Error);
        Assert.Single(binder.Documents, document => document.Id == documentId);
        Assert.Single(binder.DomainEvents, domainEvent => domainEvent is DocumentAdded documentAdded
            && documentAdded.Id == id
            && documentAdded.DocumentId == documentId
        );
    }

    [Fact]
    public void RequestComposition_ShouldUpdateStatusAndRaiseEvent_WhenBinderInValidState()
    {
        var id = BinderId.Create();
        var name = "Test Binder";
        var binder = Domain.Binder.Binder.Create(id, name).Value;

        var documentId = DocumentId.Create();
        var sourceUri = StorageUri.From("file://home/ryan/Downloads/document.txt").Value;
        var mimeType = new ContentType(MediaTypeNames.Text.Plain);

        binder.AddDocument(documentId, sourceUri, mimeType);
        var result = binder.RequestComposition();

        Assert.True(result.IsSuccessful);
        Assert.Equal(BinderStatus.ReadyForComposition, binder.Status);
        Assert.Single(binder.DomainEvents, domainEvent => domainEvent is CompositionRequested compositionRequested
            && compositionRequested.Id == id
        );
    }

    [Fact]
    public void RequestComposition_ShouldFail_WhenBinderNotInValidState()
    {
        var id = BinderId.Create();
        var name = "Test Binder";
        var binder = Domain.Binder.Binder.Create(id, name).Value;

        var documentId = DocumentId.Create();
        var sourceUri = StorageUri.From("file://home/ryan/Downloads/document.txt").Value;
        var mimeType = new ContentType(MediaTypeNames.Text.Plain);

        binder.AddDocument(documentId, sourceUri, mimeType);
        binder.RequestComposition();
        var result = binder.RequestComposition();

        Assert.False(result.IsSuccessful);
        Assert.Equal(BinderStatus.ReadyForComposition, binder.Status);
        Assert.Single(binder.DomainEvents, domainEvent => domainEvent is CompositionRequested compositionRequested
            && compositionRequested.Id == id
        );
    }
}
