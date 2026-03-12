using System.Net.Http.Json;
using DocumentComposition.Application.Binders;
using DocumentComposition.Integration.Tests.Fixtures.Api;

namespace DocumentComposition.Integration.Tests.Fixtures.Binders;

[Collection("Api Collection")]
public class ExistingBinderFixture(ApiFixture apiFixture) : IAsyncLifetime
{
    public Guid Id { get; private set; }

    public async Task InitializeAsync()
    {
        var response = await apiFixture.Client.PostAsJsonAsync("/api/binders", new { Name = "Test Binder" });
        response.EnsureSuccessStatusCode();

        var dtoResult = await response.Content.ReadFromJsonAsync<BinderCreatedDto>();
        Id = dtoResult!.Id;
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
