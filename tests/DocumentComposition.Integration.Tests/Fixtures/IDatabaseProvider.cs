namespace DocumentComposition.Integration.Tests.Fixtures;

public interface IDatabaseProvider
{
    string ConnectionString { get; }
    Task StartAsync();
    Task StopAsync();
    Task<Dictionary<string, object?>?> QueryRowAsync(string query, object? parameters = null);
}
