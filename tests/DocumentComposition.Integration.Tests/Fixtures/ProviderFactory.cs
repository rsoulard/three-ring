using DocumentComposition.Integration.Tests.Fixtures.Providers;

namespace DocumentComposition.Integration.Tests.Fixtures;

public static class ProviderFactory
{
    public static IDatabaseProvider Create()
    {
        var provider = Environment.GetEnvironmentVariable("TEST_DB_PROVIDER")?.ToLowerInvariant();

        return provider switch
        {
            "sqlite" or _ => new SqliteProvider()
        };
    }
}
