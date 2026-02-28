using DocumentComposition.Domain.Binder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentComposition.Infrastructure.Binders;

internal class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("Documents");
        builder.HasKey(d => d.Id);

        builder.Property(binder => binder.Id)
            .HasConversion<DocumentIdConverter>();

        builder.Property(document => document.Order)
            .HasConversion<DocumentOrderConverter>();

        builder.Property(document => document.SourceUri)
            .HasConversion<StorageUriConverter>();

        builder.Property(document => document.MimeType)
            .HasConversion<ContentTypeConverter>();
    }
}