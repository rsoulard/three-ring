using DocumentComposition.Application.Binders;
using DocumentComposition.Application.Data;
using DocumentComposition.Infrastructure.Binders;
using DocumentComposition.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentComposition.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDocumentComposition(this IServiceCollection services, Action<DocumentCompositionInfrastructureOptions> configureOptions)
    {
        var options = new DocumentCompositionInfrastructureOptions();
        configureOptions(options);

        if (string.IsNullOrEmpty(options.ConnectionString))
        {
            throw new ArgumentException("Connection string must be provided.");
        }

        services.AddDbContext<DocumentCompositionDbContext>(dbOptions =>
        {
            dbOptions.UseSqlite(options.ConnectionString);
        });

        services.AddTransient<MigrationService<DocumentCompositionDbContext>>();

        services.AddScoped<IUnitOfWork, EfCoreUnitOfWork<DocumentCompositionDbContext>>();

        services.AddBindersApplicationModule()
            .AddBindersInfrastructureModule();

        return services;
    }
}
