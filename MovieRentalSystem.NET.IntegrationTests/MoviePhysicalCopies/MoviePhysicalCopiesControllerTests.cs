using System.Net;
using System.Net.Http.Json;
using MovieRentalSystem.NET.IntegrationTests.Fixtures;
using MovieRentalSystem.NET.IntegrationTests.Helpers;

namespace MovieRentalSystem.NET.IntegrationTests.MoviePhysicalCopies;

[Collection("Aspire collection")]
public class MoviePhysicalCopiesControllerTests
{
    private readonly AspireFixture _fixture;

    public MoviePhysicalCopiesControllerTests(
        AspireFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Admin_CanGetMoviePhysicalCopies()
    {
        // Arrange
        var client = _fixture.CreateClient();

        var auth =
            new AuthTestHelper(client);

        var login =
            await auth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            login.IsSuccessStatusCode);

        // Act
        var response =
            await client.GetAsync(
                "/api/moviePhysicalCopies");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Customer_CannotGetMoviePhysicalCopies()
    {
        // Arrange
        var user =
            TestUserData.Customer();

        var auth =
            _fixture.CreateAuthHelper();

        var login =
            await auth.RegisterAndLoginAsync(user);

        Assert.True(
            login.IsSuccessStatusCode);

        // Act
        var response =
            await _fixture.ApiClient
                .GetAsync(
                    "/api/moviePhysicalCopies");

        // Assert
        Assert.Equal(
            HttpStatusCode.Forbidden,
            response.StatusCode);
    }

    [Fact]
    public async Task AnonymousUser_CanGetMoviePhysicalCopies()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        // Act
        var response =
            await client.GetAsync(
                "/api/moviePhysicalCopies");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }


    [Fact]
    public async Task Admin_CanCreateMoviePhysicalCopy()
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
            MovieId = 1,
            Code = $"COPY-TEST-{Guid.NewGuid():N}"
        };

        // Act
        var response =
            await client.PostAsJsonAsync(
                "/api/moviePhysicalCopies",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);
    }

    [Fact]
    public async Task Customer_CannotCreateMoviePhysicalCopy()
    {
        // Arrange
        var user =
            TestUserData.Customer();

        var auth =
            _fixture.CreateAuthHelper();

        var login =
            await auth.RegisterAndLoginAsync(user);

        Assert.True(
            login.IsSuccessStatusCode);

        var request = new
        {
            MovieId = 1,
            Code = "COPY-CUSTOMER"
        };

        // Act
        var response =
            await _fixture.ApiClient
                .PostAsJsonAsync(
                    "/api/moviePhysicalCopies",
                    request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Forbidden,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_CreateMoviePhysicalCopy_WithInvalidMovie_ReturnsNotFound()
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
            MovieId = 999999,
            Code = $"INVALID-{Guid.NewGuid():N}"
        };

        // Act
        var response =
            await client.PostAsJsonAsync(
                "/api/moviePhysicalCopies",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_CanUpdateMoviePhysicalCopy()
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
            login.IsSuccessStatusCode,
            "Admin login failed.");

        var request = new
        {
            Status = 1
        };

        // Act
        var response =
            await client.PutAsJsonAsync(
                "/api/moviePhysicalCopies/1/1",
                request);

        // Assert
        var responseBody =
            await response.Content.ReadAsStringAsync();

        Assert.True(
            response.StatusCode == HttpStatusCode.NoContent,
            $"Expected 204 No Content, but received " +
            $"{(int) response.StatusCode} {response.StatusCode}. " +
            $"Response: {responseBody}");
    }

    [Fact]
    public async Task Admin_UpdateNonExistingCopy_ReturnsNotFound()
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

        var request = new
        {
            Status = 1
        };

        // Act
        var response =
            await _fixture.ApiClient
                .PutAsJsonAsync(
                    "/api/moviePhysicalCopies/999999/1",
                    request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }


    [Fact]
    public async Task Admin_CanDeleteMoviePhysicalCopy()
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

        // Create a new physical copy specifically for this test
        var createRequest = new
        {
            MovieId = 1,
            Code = $"COPY-DELETE-{Guid.NewGuid():N}"
        };

        var createResponse =
            await _fixture.ApiClient
                .PostAsJsonAsync(
                    "/api/moviePhysicalCopies",
                    createRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            createResponse.StatusCode);

        var createdCopy =
            await createResponse.Content
                .ReadFromJsonAsync<MoviePhysicalCopyTestDto>();

        Assert.NotNull(createdCopy);
        Assert.True(createdCopy.Id > 0);

        // Act
        var deleteResponse =
            await _fixture.ApiClient
                .DeleteAsync(
                    $"/api/moviePhysicalCopies/{createdCopy.Id}/1");

        // Assert
        var responseBody =
            await deleteResponse.Content
                .ReadAsStringAsync();

        Assert.True(
            deleteResponse.StatusCode == HttpStatusCode.NoContent,
            $"Expected 204 No Content, but received " +
            $"{(int) deleteResponse.StatusCode} " +
            $"{deleteResponse.StatusCode}. " +
            $"Response: {responseBody}");
    }

    private sealed class MoviePhysicalCopyTestDto
    {
        public int Id { get; set; }
    }

    [Fact]
    public async Task Admin_DeleteNonExistingCopy_ReturnsNotFound()
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
                    "/api/moviePhysicalCopies/999999/1");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}
