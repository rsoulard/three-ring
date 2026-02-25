using DocumentComposition.Domain.Binder;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Domain.Tests.Binder;

public class DocumentIdTests
{
    [Fact]
    public void Create_ShouldCreateNonEmptyGuid()
    {
        var id = DocumentId.Create();

        Assert.NotEqual(Guid.Empty, id.Value);
    }

    [Fact]
    public void FromGuid_ShouldParse_WhenGuidIsValid()
    {
        var guid = Guid.CreateVersion7();
        var idResult = DocumentId.From(guid);

        Assert.True(idResult.IsSuccessful);
        Assert.Equal(guid, idResult.Value.Value);
    }

    [Fact]
    public void FromGuid_ShouldFail_WhenGuidIsEmpty()
    {
        var guid = Guid.Empty;
        var idResult = DocumentId.From(guid);

        Assert.False(idResult.IsSuccessful);
        Assert.IsType<ValidationError>(idResult.Error);
    }

    [Fact]
    public void FromString_ShouldParse_WhenStringIsValidGuid()
    {
        var guidString = Guid.CreateVersion7().ToString();
        var idResult = DocumentId.From(guidString);

        Assert.True(idResult.IsSuccessful);
        Assert.Equal(guidString, idResult.Value.Value.ToString());
    }

    [Fact]
    public void FromGuid_ShouldFail_WhenStringInvalidGuid()
    {
        var guidString = "TotallyNotAGuid";
        var idResult = DocumentId.From(guidString);

        Assert.False(idResult.IsSuccessful);
        Assert.IsType<ValidationError>(idResult.Error);
    }

    [Fact]
    public void Equals_ShouldBeTrue_ForSameGuid()
    {
        var guid = Guid.CreateVersion7();
        var id1 = DocumentId.From(guid).Value;
        var id2 = DocumentId.From(guid).Value;

        Assert.Equal(id1, id2);
        Assert.True(id1.Equals(id2));
        Assert.Equal(id1.GetHashCode(), id2.GetHashCode());
    }

    [Fact]
    public void Equals_ShouldBeFalse_ForDifferentGuid()
    {
        var id1 = DocumentId.Create();
        var id2 = DocumentId.Create();

        Assert.NotEqual(id1, id2);
        Assert.False(id1.Equals(id2));
        Assert.NotEqual(id1.GetHashCode(), id2.GetHashCode());
    }

    [Fact]
    public void ToString_ShouldReturnUnderlyingGuid()
    {
        var guid = Guid.CreateVersion7();
        var id = DocumentId.From(guid).Value;

        var result = id.ToString();

        Assert.Equal(guid.ToString(), result);
    }
}
