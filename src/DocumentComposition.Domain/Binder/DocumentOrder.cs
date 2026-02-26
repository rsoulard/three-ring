using DocumentComposition.Shared.Results;

namespace DocumentComposition.Domain.Binder;

public sealed class DocumentOrder : IEquatable<DocumentOrder>, IComparable<DocumentOrder>
{
    public int Value { get; }

    private DocumentOrder(int value)
    {
        Value = value;
    }

    public static Result<DocumentOrder> From(int value)
    {
        if (value <= 0)
        {
            return Result.Failed<DocumentOrder>(new ValidationError("documentOrder", "Document order must be a positive number."));
        }

        return Result.From(new DocumentOrder(value));
    }

    public int CompareTo(DocumentOrder? other) => other is null ? 1 : Value.CompareTo(other.Value);

    public bool Equals(DocumentOrder? other) => other is not null && Value == other.Value;

    public override bool Equals(object? obj) => obj is DocumentOrder other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();
}
