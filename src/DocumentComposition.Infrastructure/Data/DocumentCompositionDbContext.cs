using Microsoft.EntityFrameworkCore;

namespace DocumentComposition.Infrastructure.Data;

public class DocumentCompositionDbContext : DbContext
{
    public DocumentCompositionDbContext(DbContextOptions<DocumentCompositionDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentCompositionDbContext).Assembly);
    }
}
