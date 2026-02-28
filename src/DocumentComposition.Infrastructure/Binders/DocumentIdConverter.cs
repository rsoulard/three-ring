using DocumentComposition.Domain.Binder;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DocumentComposition.Infrastructure.Binders;

internal class DocumentIdConverter : ValueConverter<DocumentId, Guid>
{
    public DocumentIdConverter() : base(
        id => id.Value,
        value => DocumentId.From(value).Value
    )
    {
    }
}
