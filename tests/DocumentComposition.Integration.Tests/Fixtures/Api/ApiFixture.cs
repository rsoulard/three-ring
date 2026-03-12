namespace DocumentComposition.Integration.Tests.Fixtures.Api;

public class ApiFixture : IAsyncLifetime
{
    public IDatabaseProvider Database { get; private set; } = default!;
    public TestApiFactory<Program> Factory { get; private set; } = default!;
    public HttpClient Client { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        Database = ProviderFactory.Create();
        await Database.StartAsync();

        Factory = new TestApiFactory<Program>(Database);
        Client = Factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        Client.Dispose();
        Factory.Dispose();
        await Database.StopAsync();
    }
}
