
using Microsoft.Data.Sqlite;

namespace DocumentComposition.Integration.Tests.Fixtures.Providers;

public class SqliteProvider : IDatabaseProvider
{
    private string dbPath = default!;
    private SqliteConnection connection = default!;

    public string ConnectionString => connection.ConnectionString;

    public Task QueryAsync<TResult>(string query)
    {
        throw new NotImplementedException();
    }

    public async Task StartAsync()
    {
        dbPath = Path.GetTempFileName();
        connection = new($"Data Source={dbPath}");
    }

    public async Task StopAsync()
    {
        await connection.DisposeAsync();

        if (File.Exists(dbPath))
        {
            File.Delete(dbPath);
        }
    }

    public async Task<Dictionary<string, object?>?> QueryRowAsync(string query, object? parameters = null)
    {
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = query;

        if (parameters is not null)
        {
            foreach (var property in parameters.GetType().GetProperties())
            {
                command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(parameters) ?? DBNull.Value);
            }
        }

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return default;
        }

        return Map(reader);
    }

    private static Dictionary<string, object?> Map(SqliteDataReader reader)
    {
        var dictionary = new Dictionary<string, object?>();

        for (int i = 0; i < reader.FieldCount; i++)
        {
            var name = reader.GetName(i);
            var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
            dictionary[name] = value;
        }

        return dictionary;
    }
}
