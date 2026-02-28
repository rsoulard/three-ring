using DocumentComposition.Domain.Binder;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DocumentComposition.Infrastructure.Binders;

internal class BinderIdConverter : ValueConverter<BinderId, Guid>
{
    public BinderIdConverter() : base(
        id => id.Value,
        value => BinderId.From(value).Value
    )
    {
    }
}
