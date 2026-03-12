using System.Net.Http.Json;
using DocumentComposition.Integration.Tests.Fixtures.Api;
using DocumentComposition.Integration.Tests.Fixtures.Binders;

namespace DocumentComposition.Integration.Tests.Scenarios.Binders;

[Collection("Api Collection")]
public class AddDocumentTests(ApiFixture api, ExistingBinderFixture existingBinder) : IClassFixture<ExistingBinderFixture>
{
    [Fact]
    public async Task AddDocument_ShouldPersistAndReturnSuccess_WhenRequestIsValid()
    {
        var response = await api.Client.PostAsJsonAsync($"/api/binders/{existingBinder.Id}/add-document", new { SourceUri = "file://tmp/image.png" });
        response.EnsureSuccessStatusCode();

        var databaseResult = await api.Database.QueryRowsAsync("SELECT * FROM Documents WHERE BinderId = @BinderId AND SourceUri = @SourceUri", new { BinderId = existingBinder.Id, SourceUri = "file://tmp/image.png" });
        Assert.Single(databaseResult, row => (string)row["SourceUri"]! == "file://tmp/image.png");
    }

    [Fact]
    public async Task AddDocument_ShouldReturnBadRequest_WhenSourceUriIsNotValid()
    {

    }
}
