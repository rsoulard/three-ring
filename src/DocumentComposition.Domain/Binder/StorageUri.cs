using DocumentComposition.Shared.Results;

namespace DocumentComposition.Domain.Binder;

public sealed class StorageUri : IEquatable<StorageUri>
{
    public Uri Value { get; } = null!;
    public string Scheme => Value.Scheme;

    private StorageUri(Uri uri)
    {
        Value = uri;
    }

    public static Result<StorageUri> From(string uri)
    {
        if (string.IsNullOrWhiteSpace(uri))
        {
            return Result.Failed<StorageUri>(new ValidationError("storageUri", "Storage URI cannot be null or empty."));
        }

        if (!Uri.TryCreate(uri, UriKind.Absolute, out var parsed))
        {
            return Result.Failed<StorageUri>(new ValidationError("storageUri", $"Storage URI must be an absolute URI: '{uri}"));
        }

        return Result.From(new StorageUri(parsed));
    }

    public static Result<StorageUri> From(Uri uri)
    {
        if (uri is null)
        {
            return Result.Failed<StorageUri>(new ValidationError("storageUri", "Storage URI cannot be null or empty."));
        }

        if (!uri.IsAbsoluteUri)
        {
            return Result.Failed<StorageUri>(new ValidationError("storageUri", $"Storage URI must be an absolute URI: '{uri}"));
        }

        return Result.From(new StorageUri(uri));
    }

    public override string ToString() => Value.ToString();

    public bool Equals(StorageUri? other) => other is not null && Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is StorageUri other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();
}
