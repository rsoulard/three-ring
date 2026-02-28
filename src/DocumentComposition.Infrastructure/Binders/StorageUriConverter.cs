using DocumentComposition.Domain.Binder;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DocumentComposition.Infrastructure.Binders;

internal class StorageUriConverter : ValueConverter<StorageUri, string>
{
    public StorageUriConverter() : base(
        uri => uri.ToString(),
        value => StorageUri.From(value).Value
    )
    {
    }
}
