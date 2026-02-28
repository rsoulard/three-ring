using DocumentComposition.Domain.Binder;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentComposition.Infrastructure.Binders;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBindersInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IBinderRepository, BinderRepository>();

        return services;
    }
}
