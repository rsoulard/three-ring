using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DocumentComposition.Integration.Tests.Fixtures.Api;

public class TestApiFactory<TEntryPoint>(IDatabaseProvider databaseProvider) : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            var overrides = new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = databaseProvider.ConnectionString
            };

            config.AddInMemoryCollection(overrides);
        });

        return base.CreateHost(builder);
    }
}
