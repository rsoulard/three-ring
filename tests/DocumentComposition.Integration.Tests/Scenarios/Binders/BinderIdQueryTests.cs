using System.Net;
using System.Net.Http.Json;
using DocumentComposition.Application.Binders;
using DocumentComposition.Integration.Tests.Fixtures.Api;

namespace DocumentComposition.Integration.Tests.Scenarios.Binders;

public class BinderIdQueryTests(ApiFixture apiFixture) : IClassFixture<ApiFixture>
{
    [Fact]
    public async Task BinderIdQuery_ShouldReturnDto_WhenBinderExists()
    {
        var createResponse = await apiFixture.Client.PostAsJsonAsync("/api/binders", new { Name = "Test Binder" });
        createResponse.EnsureSuccessStatusCode();

        var createdDtoResult = await createResponse.Content.ReadFromJsonAsync<BinderCreatedDto>();
        var binderId = createdDtoResult!.Id;

        var response = await apiFixture.Client.GetAsync($"/api/binders/{binderId}");

        var dtoResult = await response.Content.ReadFromJsonAsync<BinderDto>();
        Assert.Equal(binderId, dtoResult!.Id);
        Assert.Equal("Test Binder", dtoResult.Name);
    }

    [Fact]
    public async Task BinderIdQuery_ShouldReturnNotFound_WhenBinderDoesNotExists()
    {
        var response = await apiFixture.Client.GetAsync($"/api/binders/{Guid.CreateVersion7()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
