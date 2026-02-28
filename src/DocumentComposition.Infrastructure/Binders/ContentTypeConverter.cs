using System.Net.Mime;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DocumentComposition.Infrastructure.Binders;

internal class ContentTypeConverter : ValueConverter<ContentType, string>
{
    public ContentTypeConverter() : base(
        contentType => contentType.ToString(),
        value => new ContentType(value)
    )
    {
    }
}
