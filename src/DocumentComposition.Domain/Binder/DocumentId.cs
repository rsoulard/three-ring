using DocumentComposition.Shared.Results;

namespace DocumentComposition.Domain.Binder;

public sealed class DocumentId : IEquatable<DocumentId>
{
    public Guid Value { get; }

    private DocumentId(Guid value)
    {
        Value = value;
    }

    public static DocumentId Create() => new(Guid.CreateVersion7());

    public static Result<DocumentId> From(Guid value)
    {
        if (value == Guid.Empty)
        {
            return Result.Failed<DocumentId>(new ValidationError("documentId", "DocumentId cannot be empty."));
        }

        return Result.From(new DocumentId(value));
    }

    public static Result<DocumentId> From(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            return Result.Failed<DocumentId>(new ValidationError("documentId", $"Invalid DocumentId format: '{value}'"));
        }

        return From(guid);
    }

    public override string ToString() => Value.ToString();

    public bool Equals(DocumentId? other) => other is not null && Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is DocumentId other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();
}
