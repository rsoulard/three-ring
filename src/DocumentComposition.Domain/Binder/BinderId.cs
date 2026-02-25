using DocumentComposition.Shared.Results;

namespace DocumentComposition.Domain.Binder;

public sealed class BinderId : IEquatable<BinderId>
{
    public Guid Value { get; }

    private BinderId(Guid value)
    {
        Value = value;
    }

    public static BinderId Create() => new(Guid.CreateVersion7());

    public static Result<BinderId> From(Guid value)
    {
        if (value == Guid.Empty)
        {
            return Result.Failed<BinderId>(new ValidationError("binderId", "BinderId cannot be empty."));
        }

        return Result.From(new BinderId(value));
    }

    public static Result<BinderId> From(string value)
    {
        if (!Guid.TryParse(value, out var guid))
        {
            return Result.Failed<BinderId>(new ValidationError("binderId", $"Invalid BinderId format: '{value}'"));
        }

        return From(guid);
    }

    public override string ToString() => Value.ToString();

    public bool Equals(BinderId? other) => other is not null && Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is BinderId other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();
}
