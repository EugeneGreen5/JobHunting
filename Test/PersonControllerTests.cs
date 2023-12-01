using Bogus;
using JobHunting.Models.Entity;
using Microsoft.OpenApi.Validations;
using System.Net;
using System.Net.Http.Json;
using Xunit;


namespace Test;
public class PersonControllerTests : 
    IClassFixture<DockerFactoryFixture>
{
    private readonly DockerFactoryFixture _factory;
    private HttpClient _client;

    public PersonControllerTests(DockerFactoryFixture factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task AuthAsync_RegisteredUser_ShouldReturnOk()
    {
        // Arrange
        var person = GetPerson();

        // Act
        using var responsePost = await _client.PostAsJsonAsync("/api/person", person);

        using var response = await _client.PostAsync($"/api/person/auth?name={person.Name}&password={person.Password}", null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);  
    }

    [Theory]
    [InlineData("string", "string")]
    [InlineData("admin", "admin")]
    public async Task AuthAsync_NonExistUser_ShouldReturnNotFound(string name, string password)
    {
        // Act
        using var response = await _client.PostAsync($"/api/person/auth?name={name}&password={password}", null);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("/api/person")]
    [InlineData("/api/person/{14ada16c-64e0-448a-8cf5-deb87f68f53b}")]
    public async Task GetPersonByIdAsync_NonExistUser_ShouldReturnUnauthorized(string path)
    {
        // Act
        using var response = await _client.GetAsync(path);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private JobHunting.Models.Entity.Person GetPerson() => new JobHunting.Models.Entity.Person
    {
        Id = Guid.NewGuid(),
        Name = _faker.Internet.UserName(),
        Password = _faker.Internet.Password(),
        Email = _faker.Internet.Email(),
        Phone = _faker.Phone.PhoneNumber()
    };

    private Faker _faker = new Faker("ru");
}
