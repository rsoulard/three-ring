using System.Net;
using System.Net.Http.Json;
using DocumentComposition.Application.Binders;
using DocumentComposition.Integration.Tests.Fixtures.Api;
using DocumentComposition.Integration.Tests.Fixtures.Binders;

namespace DocumentComposition.Integration.Tests.Scenarios.Binders;

[Collection("Api Collection")]
public class BinderIdQueryTests(ApiFixture api, ExistingBinderFixture existingBinder) : IClassFixture<ExistingBinderFixture>
{
    [Fact]
    public async Task BinderIdQuery_ShouldReturnDto_WhenBinderExists()
    {
        var response = await api.Client.GetAsync($"/api/binders/{existingBinder.Id}");

        var dtoResult = await response.Content.ReadFromJsonAsync<BinderDto>();
        Assert.Equal(existingBinder.Id, dtoResult!.Id);
        Assert.Equal("Test Binder", dtoResult.Name);
    }

    [Fact]
    public async Task BinderIdQuery_ShouldReturnNotFound_WhenBinderDoesNotExists()
    {
        var response = await api.Client.GetAsync($"/api/binders/{Guid.CreateVersion7()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
