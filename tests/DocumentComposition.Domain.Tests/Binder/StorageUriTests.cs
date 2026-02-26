using DocumentComposition.Domain.Binder;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Domain.Tests.Binder;

public class StorageUriTests
{
    [Fact]
    public void FromString_ShouldParse_WhenStringIsValid()
    {
        var storageUriResult = StorageUri.From("file://home/ryan/Downloads/document.txt");

        Assert.True(storageUriResult.IsSuccessful);
        Assert.Equal("file://home/ryan/Downloads/document.txt", storageUriResult.Value.Value.ToString());
    }

    [Fact]
    public void FromString_ShouldFail_WhenStringIsEmpty()
    {
        var storageUriResult = StorageUri.From("");

        Assert.False(storageUriResult.IsSuccessful);
        Assert.IsType<ValidationError>(storageUriResult.Error);
    }

    [Fact]
    public void FromString_ShouldFail_WhenStringIsNotAbsoluteUri()
    {
        var storageUriResult = StorageUri.From("document.txt");

        Assert.False(storageUriResult.IsSuccessful);
        Assert.IsType<ValidationError>(storageUriResult.Error);
    }

    [Fact]
    public void FromUri_ShouldParse_WhenUriIsValid()
    {
        var storageUriResult = StorageUri.From(new Uri("file://home/ryan/Downloads/document.txt"));

        Assert.True(storageUriResult.IsSuccessful);
        Assert.Equal("file://home/ryan/Downloads/document.txt", storageUriResult.Value.Value.ToString());
    }

    [Fact]
    public void FromUri_ShouldFail_WhenUriIsNotAbsoluteUri()
    {
        var storageUriResult = StorageUri.From(new Uri("/home/ryan/Downloads/document.txt", UriKind.Relative));

        Assert.False(storageUriResult.IsSuccessful);
        Assert.IsType<ValidationError>(storageUriResult.Error);
    }

    [Fact]
    public void Equals_ShouldBeTrue_ForSameUri()
    {
        var uri = new Uri("file://home/ryan/Downloads/document.txt");
        var storageUri1 = StorageUri.From(uri).Value;
        var storageUri2 = StorageUri.From(uri).Value;

        Assert.Equal(storageUri1, storageUri2);
        Assert.True(storageUri1.Equals(storageUri2));
        Assert.Equal(storageUri1.GetHashCode(), storageUri2.GetHashCode());
    }

    [Fact]
    public void Equals_ShouldBeFalse_ForDifferentUri()
    {
        var storageUri1 = StorageUri.From(new Uri("file://home/ryan/Downloads/document.txt")).Value;
        var storageUri2 = StorageUri.From(new Uri("file://home/ryan/Downloads/document.png")).Value;

        Assert.NotEqual(storageUri1, storageUri2);
        Assert.False(storageUri1.Equals(storageUri2));
        Assert.NotEqual(storageUri1.GetHashCode(), storageUri2.GetHashCode());
    }

    [Fact]
    public void ToString_ShouldReturnUnderlyingUri()
    {
        var uri = new Uri("file://home/ryan/Downloads/document.txt");
        var storageUri = StorageUri.From(uri).Value;

        var result = storageUri.ToString();

        Assert.Equal(uri.ToString(), result);
    }
}
