namespace DocumentComposition.Api.Binders;

public static class BindersApiModule
{
    public static IServiceCollection AddBindersApiModule(this IServiceCollection services)
    {
        services.AddTransient<CreateBinderCommandMapper>();
        services.AddTransient<BinderIdQueryMapper>();
        services.AddTransient<AddDocumentCommandMapper>();

        return services;
    }
}
