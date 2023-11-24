using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;


namespace Test;
public class PersonControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public PersonControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("string","string")]
    [InlineData("admin", "admin")]
    public async Task CheckStatus_ShouldReturnOk(string name, string password)
    {
        var client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsync($"/api/person/auth?name={name}&password={password}", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);  
    }

    [Theory]
    [InlineData("321", "123")]
    [InlineData("dsds", "123")]
    public async Task CheckStatus_ShouldReturnNotFound(string name, string password)
    {
        var client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsync($"/api/person/auth?name={name}&password={password}", null);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("/api/person")]
    [InlineData("/api/person/{14ada16c-64e0-448a-8cf5-deb87f68f53b}")]
    public async Task CheckStatus_ShouldReturnUnauthorized(string path)
    {
        var client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync(path);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


}
