using DocumentComposition.Domain.Binder;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Domain.Tests.Binder;

public class DocumentOrderTests
{
    [Fact]
    public void FromInt_ShouldParse_WhenIntIsValid()
    {
        var documentOrderResult = DocumentOrder.From(2);

        Assert.True(documentOrderResult.IsSuccessful);
        Assert.Equal(2, documentOrderResult.Value.Value);
    }

    [Fact]
    public void FromInt_ShouldFail_WhenIntIsLessThanOne()
    {
        var documentOrderResult = DocumentOrder.From(-1);

        Assert.False(documentOrderResult.IsSuccessful);
        Assert.IsType<ValidationError>(documentOrderResult.Error);
    }

    [Fact]
    public void CompareTo_ShouldReturnNegativeOne_WhenOtherIsHigher()
    {
        var documentOrder1 = DocumentOrder.From(2).Value;
        var documentOrder2 = DocumentOrder.From(3).Value;

        var result = documentOrder1.CompareTo(documentOrder2);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareTo_ShouldReturnOne_WhenOtherIsLower()
    {
        var documentOrder1 = DocumentOrder.From(2).Value;
        var documentOrder2 = DocumentOrder.From(1).Value;

        var result = documentOrder1.CompareTo(documentOrder2);

        Assert.Equal(1, result);
    }

    [Fact]
    public void CompareTo_ShouldReturnZero_WhenOtherIsEqual()
    {
        var documentOrder1 = DocumentOrder.From(2).Value;
        var documentOrder2 = DocumentOrder.From(2).Value;

        var result = documentOrder1.CompareTo(documentOrder2);

        Assert.Equal(0, result);
    }

    [Fact]
    public void Equals_ShouldBeTrue_ForSameInt()
    {
        var documentOrder1 = DocumentOrder.From(2).Value;
        var documentOrder2 = DocumentOrder.From(2).Value;

        Assert.Equal(documentOrder1, documentOrder2);
        Assert.True(documentOrder1.Equals(documentOrder2));
        Assert.Equal(documentOrder1.GetHashCode(), documentOrder2.GetHashCode());
    }

    [Fact]
    public void Equals_ShouldBeFalse_ForDifferentInt()
    {
        var documentOrder1 = DocumentOrder.From(2).Value;
        var documentOrder2 = DocumentOrder.From(3).Value;

        Assert.NotEqual(documentOrder1, documentOrder2);
        Assert.False(documentOrder1.Equals(documentOrder2));
        Assert.NotEqual(documentOrder1.GetHashCode(), documentOrder2.GetHashCode());
    }
}
