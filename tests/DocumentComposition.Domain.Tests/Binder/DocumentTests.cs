using System.Net.Mime;
using DocumentComposition.Domain.Binder;

namespace DocumentComposition.Domain.Tests.Binder;

public class DocumentTests
{
    [Fact]
    public void Create_ShouldCreateDocument()
    {
        var id = DocumentId.Create();
        var order = DocumentOrder.From(1).Value;
        var sourceUri = StorageUri.From("file://home/ryan/Downloads/document.txt").Value;
        var mimeType = new ContentType(MediaTypeNames.Text.Plain);

        var result = Document.Create(id, order, sourceUri, mimeType);

        Assert.True(result.IsSuccessful);
        Assert.Equal(order, result.Value.Order);
        Assert.Equal(sourceUri, result.Value.SourceUri);
        Assert.Equal(mimeType, result.Value.MimeType);
    }
}
