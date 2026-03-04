using DocumentComposition.Domain.Binder;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentComposition.Infrastructure.Binders;

public static class BindersInfrastructureModule
{
    public static IServiceCollection AddBindersInfrastructureModule(this IServiceCollection services)
    {
        services.AddScoped<IBinderRepository, BinderRepository>();

        return services;
    }
}
