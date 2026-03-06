using Microsoft.EntityFrameworkCore;

namespace DocumentComposition.Infrastructure.Data;

internal class MigrationService<TDbContext>(TDbContext dbContext) where TDbContext : DbContext
{
    public void Migrate()
    {
        dbContext.Database.Migrate();
    }
}
