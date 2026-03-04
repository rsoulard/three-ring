using Microsoft.Extensions.DependencyInjection;

namespace DocumentComposition.Application.Binders;


public static class BindersApplicationModule
{
    public static IServiceCollection AddBindersApplicationModule(this IServiceCollection services)
    {
        services.AddScoped<CreateBinderCommandHandler>();

        return services;
    }
}
