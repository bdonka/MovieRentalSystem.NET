using System.Net;
using System.Net.Http.Json;
using MovieRentalSystem.NET.IntegrationTests.Fixtures;
using MovieRentalSystem.NET.IntegrationTests.Helpers;

namespace MovieRentalSystem.NET.IntegrationTests.Movies;

[Collection("Aspire collection")]
public class MoviesControllerTests
{
    private readonly AspireFixture _fixture;

    public MoviesControllerTests(AspireFixture fixture)
    {
        _fixture = fixture;
    }


    [Fact]
    public async Task GetMovies_ReturnsOk_WithMovies()
    {
        // Arrange
        var client = _fixture.ApiClient;

        // Act
        var response =
            await client.GetAsync("/api/movies");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var body =
            await response.Content.ReadAsStringAsync();

        Assert.NotNull(body);
    }


    [Fact]
    public async Task GetMovie_WithExistingId_ReturnsOk()
    {
        // Arrange
        var client = _fixture.ApiClient;

        // Act
        var response =
            await client.GetAsync("/api/movies/1");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }


    [Fact]
    public async Task GetMovie_WithNonExistingId_ReturnsNotFound()
    {
        // Arrange
        var client = _fixture.ApiClient;

        // Act
        var response =
            await client.GetAsync("/api/movies/999999");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_CanCreateMovie()
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
            Title = "Test Movie",
            Description = "Integration test movie",
            ReleaseYear = 2025,
            RentalPrice = 20m
        };

        // Act
        var response =
            await client.PostAsJsonAsync(
                "/api/movies",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);
    }

    [Fact]
    public async Task Customer_CannotCreateMovie()
    {
        // Arrange
        var user =
            TestUserData.Customer();

        var auth =
            _fixture.CreateAuthHelper();

        await auth.RegisterAndLoginAsync(user);


        var request = new
        {
            Title = "Customer Movie",
            Description = "Should fail",
            ReleaseYear = 2025,
            RentalPrice = 10m
        };


        // Act
        var response =
            await _fixture.ApiClient
                .PostAsJsonAsync(
                    "/api/movies",
                    request);


        // Assert
        Assert.Equal(
            HttpStatusCode.Forbidden,
            response.StatusCode);
    }

    [Fact]
    public async Task AnonymousUser_CanCreateMovie()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        var request = new
        {
            Title = $"Anonymous Movie {Guid.NewGuid():N}",
            Description = "Should succeed",
            ReleaseYear = 2025,
            RentalPrice = 10m
        };

        // Act
        var response =
            await client.PostAsJsonAsync(
                "/api/movies",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_CanUpdateMovie()
    {
        // Arrange
        var client = _fixture.ApiClient;

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
            Title = "Updated Movie",
            Description = "Updated description",
            ReleaseYear = 2026,
            RentalPrice = 30m
        };

        // Act
        var response =
            await client.PutAsJsonAsync(
                "/api/movies/1",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_DeleteMovie_ReturnsInternalServerError()
    {
        // Arrange
        var auth =
            new AuthTestHelper(_fixture.ApiClient);

        var login =
            await auth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            login.IsSuccessStatusCode);

        // Act
        var response =
            await _fixture.ApiClient
                .DeleteAsync(
                    "/api/movies/1");

        // Assert
        Assert.Equal(
            HttpStatusCode.InternalServerError,
            response.StatusCode);
    }

    [Fact]
    public async Task DeleteMovie_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var auth =
            _fixture.CreateAuthHelper();

        var login =
            await auth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            login.IsSuccessStatusCode);

        // Act
        var response =
            await _fixture.ApiClient
                .DeleteAsync(
                    "/api/movies/999999");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_CreateMovie_WithInvalidData_ReturnsInternalServerError()
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
            Title = "",
            Description = "",
            ReleaseYear = 0,
            RentalPrice = -1
        };

        // Act
        var response =
            await client.PostAsJsonAsync(
                "/api/movies",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.InternalServerError,
            response.StatusCode);
    }
}
