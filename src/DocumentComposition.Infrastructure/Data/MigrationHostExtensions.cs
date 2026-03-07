using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DocumentComposition.Infrastructure.Data;

public static class MigrationHostExtensions
{
    public static void Migrate(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var migrationService = scope.ServiceProvider.GetRequiredService<MigrationService<DocumentCompositionDbContext>>();

        migrationService.Migrate();
    }
}
