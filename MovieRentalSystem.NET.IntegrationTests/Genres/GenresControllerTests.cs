using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using MovieRentalSystem.NET.IntegrationTests.Fixtures;
using MovieRentalSystem.NET.IntegrationTests.Helpers;

namespace MovieRentalSystem.NET.IntegrationTests.Genres;

[Collection("Aspire collection")]
public class GenresControllerTests
{
    private readonly AspireFixture _fixture;
    public GenresControllerTests(AspireFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetGenres_ReturnsOk()
    {
        var client = _fixture.CreateClient();

        var response =
            await client.GetAsync("/api/genres");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task GetGenre_WithExistingId_ReturnsOk()
    {
        var client = _fixture.CreateClient();

        var response =
            await client.GetAsync("/api/genres/1");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task GetGenre_WithInvalidId_ReturnsNotFound()
    {
        var client = _fixture.CreateClient();

        var response =
            await client.GetAsync("/api/genres/999999");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_CanCreateGenre()
    {
        var client = _fixture.CreateClient();

        var auth =
            new AuthTestHelper(client);

        var login =
            await auth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            login.IsSuccessStatusCode);

        var request = new
        {
            Name = $"Comedy-{Guid.NewGuid():N}"
        };

        var response =
            await client.PostAsJsonAsync(
                "/api/genres",
                request);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);
    }

    [Fact]
    public async Task Customer_CannotCreateGenre()
    {
        // Arrange
        var client = _fixture.CreateClient();

        var auth = new AuthTestHelper(client);

        var user = TestUserData.Customer();

        var loginResponse =
            await auth.RegisterAndLoginAsync(user);

        Assert.True(
            loginResponse.IsSuccessStatusCode);

        var request = new
        {
            Name = $"Horror-{Guid.NewGuid():N}"
        };

        // Act
        var response =
            await client.PostAsJsonAsync(
                "/api/genres",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Forbidden,
            response.StatusCode);
    }

    [Fact]
    public async Task AnonymousUser_CannotCreateGenre()
        {
        // Arrange
        var client =
            _fixture.CreateAnonymousClient();

        var request = new
        {
            Name = $"Fantasy-{Guid.NewGuid():N}"
        };

        // Act
        var response =
            await client.PostAsJsonAsync(
                "/api/genres",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_CreateGenre_WithValidName_ReturnsCreated()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        var auth =
            new AuthTestHelper(client);

        var login =
            await auth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            login.IsSuccessStatusCode);

        var request = new
        {
            Name = $"Fantasy-{Guid.NewGuid():N}"
        };

        // Act
        var response =
            await client.PostAsJsonAsync(
                "/api/genres",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_CanUpdateGenre()
    {
        // Arrange
        var client =
        _fixture.CreateClient();

        var auth =
            new AuthTestHelper(client);

        var login =
            await auth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            login.IsSuccessStatusCode);

        var createRequest = new
        {
            Name = $"Genre-{Guid.NewGuid():N}"
        };

        var createResponse =
            await client.PostAsJsonAsync(
                "/api/genres",
                createRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            createResponse.StatusCode);

        var createdGenre =
            await createResponse.Content
                .ReadFromJsonAsync<JsonElement>();

        var genreId =
            createdGenre
                .GetProperty("id")
                .GetInt32();

        var updateRequest = new
        {
            Name = $"Updated-{Guid.NewGuid():N}"
        };

        // Act
        var response =
            await client.PutAsJsonAsync(
                $"/api/genres/{genreId}",
                updateRequest);

        // Assert
        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);
    }



    [Fact]
    public async Task Admin_DeleteGenre_ReturnsNoContent()
    {
        var client = _fixture.CreateClient();

        var auth =
            new AuthTestHelper(client);

        var login =
            await auth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            login.IsSuccessStatusCode);

        var response =
            await client.DeleteAsync(
                "/api/genres/1");

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_DeleteNonExistingGenre_ReturnsNotFound()
    {
        var client = _fixture.CreateClient();

        var auth =
            new AuthTestHelper(client);

        var login =
            await auth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            login.IsSuccessStatusCode);

        var response =
            await client.DeleteAsync(
                "/api/genres/999999");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}
