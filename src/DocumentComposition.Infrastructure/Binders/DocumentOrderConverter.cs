using DocumentComposition.Domain.Binder;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DocumentComposition.Infrastructure.Binders;

internal class DocumentOrderConverter : ValueConverter<DocumentOrder, int>
{
    public DocumentOrderConverter() : base(
        order => order.Value,
        value => DocumentOrder.From(value).Value
    )
    {
    }
}
