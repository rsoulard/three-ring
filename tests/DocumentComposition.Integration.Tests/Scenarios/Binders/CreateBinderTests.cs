using System.Net;
using System.Net.Http.Json;
using DocumentComposition.Application.Binders;
using DocumentComposition.Integration.Tests.Fixtures.Api;

namespace DocumentComposition.Integration.Tests.Scenarios.Binders;

public class CreateBinderTests(ApiFixture apiFixture) : IClassFixture<ApiFixture>
{
    [Fact]
    public async Task CreateBinder_ShouldPersistToDbAndReturnDto_WhenRequestIsValid()
    {
        var response = await apiFixture.Client.PostAsJsonAsync("/api/binders", new { Name = "Test Binder" });
        response.EnsureSuccessStatusCode();

        var dtoResult = await response.Content.ReadFromJsonAsync<BinderCreatedDto>();
        Assert.NotEqual(Guid.Empty, dtoResult!.Id);

        var databaseResult = await apiFixture.Database.QueryRowAsync("SELECT * FROM Binders WHERE Id = @Id", new { dtoResult.Id });
        Assert.NotNull(databaseResult);
        Assert.Equal("Test Binder", databaseResult["Name"]);
    }

    [Fact]
    public async Task CreateBinder_ShouldReturnBadRequest_WhenRequestIsNotValid()
    {
        var response = await apiFixture.Client.PostAsJsonAsync("/api/binders", new { Name = "" });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

}
