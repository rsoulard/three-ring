using DocumentComposition.Domain.Binder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentComposition.Infrastructure.Binders;

internal class BinderConfiguration : IEntityTypeConfiguration<Binder>
{
    public void Configure(EntityTypeBuilder<Binder> builder)
    {
        builder.ToTable("Binders");
        builder.HasKey(b => b.Id);

        builder.Property(binder => binder.Id)
            .HasConversion<BinderIdConverter>();

        builder.Property(binder => binder.Status)
            .HasConversion<string>();

        builder.Property(binder => binder.OutputUri)
            .HasConversion<StorageUriConverter>();

        builder.HasMany(binder => binder.Documents)
               .WithOne()
               .HasForeignKey("BinderId")
               .OnDelete(DeleteBehavior.Cascade)
               .Metadata.PrincipalToDependent!
               .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}