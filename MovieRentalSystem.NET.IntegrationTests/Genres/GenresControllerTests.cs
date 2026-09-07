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
    public async Task GetGenres_WhenGenresExist_ReturnsGenres()
    {
        // Arrange
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var admin = await _fixture.CreateAdminAsync();

        int? genreId = null;

        try
        {
            var loginResponse =
                await auth.LoginUserAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var genreName =
                $"Comedy-{Guid.NewGuid():N}";

            var createResponse =
                await client.PostAsJsonAsync(
                    "/api/genres",
                    new
                    {
                        Name = genreName
                    });

            Assert.Equal(
                HttpStatusCode.Created,
                createResponse.StatusCode);

            var createdGenre =
                await createResponse.Content
                    .ReadFromJsonAsync<JsonElement>();

            genreId =
                createdGenre
                    .GetProperty("id")
                    .GetInt32();

            // Act
            var response =
                await client.GetAsync(
                    "/api/genres");

            // Assert
            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            var genres =
                await response.Content
                    .ReadFromJsonAsync<JsonElement>();

            Assert.Equal(
                JsonValueKind.Object,
                genres.ValueKind);

            var data =
                genres.GetProperty("data");

            Assert.Equal(
                JsonValueKind.Array,
                data.ValueKind);

            Assert.Contains(
                data.EnumerateArray(),
                genre =>
                    genre.GetProperty("name").GetString()
                    == genreName);
        }
        finally
        {
            if (genreId.HasValue)
            {
                await client.DeleteAsync(
                    $"/api/genres/{genreId.Value}");
            }

            await _fixture.DeleteUserAsync(admin);
        }
    }

    [Fact]
    public async Task GetGenre_WhenGenreExists_ReturnsGenre()
    {
        // Arrange
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var admin = await _fixture.CreateAdminAsync();

        int? genreId = null;

        try
        {
            var loginResponse =
                await auth.LoginUserAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var genreName =
                $"Comedy-{Guid.NewGuid():N}";

            var createResponse =
                await client.PostAsJsonAsync(
                    "/api/genres",
                    new
                    {
                        Name = genreName
                    });

            Assert.Equal(
                HttpStatusCode.Created,
                createResponse.StatusCode);

            var createdGenre =
                await createResponse.Content
                    .ReadFromJsonAsync<JsonElement>();

            genreId =
                createdGenre
                    .GetProperty("id")
                    .GetInt32();

            // Act
            var response =
                await client.GetAsync(
                    $"/api/genres/{genreId.Value}");

            // Assert
            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            var genre =
                await response.Content
                    .ReadFromJsonAsync<JsonElement>();

            Assert.Equal(
                genreId.Value,
                genre.GetProperty("id").GetInt32());

            Assert.Equal(
                genreName,
                genre.GetProperty("name").GetString());
        }
        finally
        {
            if (genreId.HasValue)
            {
                await client.DeleteAsync(
                    $"/api/genres/{genreId.Value}");
            }

            await _fixture.DeleteUserAsync(admin);
        }
    }

    [Fact]
    public async Task GetGenre_WhenGenreDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        using var client = _fixture.CreateClient();

        var nonExistingId = int.MaxValue;

        // Act
        var response =
            await client.GetAsync(
                $"/api/genres/{nonExistingId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateGenre_WhenUserIsAdmin_ReturnsCreatedGenre()
    {
        // Arrange
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var admin = await _fixture.CreateAdminAsync();

        int? genreId = null;

        try
        {
            var loginResponse =
                await auth.LoginUserAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var genreName =
                $"Comedy-{Guid.NewGuid():N}";

            // Act
            var response =
                await client.PostAsJsonAsync(
                    "/api/genres",
                    new
                    {
                        Name = genreName
                    });

            // Assert
            Assert.Equal(
                HttpStatusCode.Created,
                response.StatusCode);

            var genre =
                await response.Content
                    .ReadFromJsonAsync<JsonElement>();

            Assert.True(
                genre.TryGetProperty(
                    "id",
                    out var id));

            genreId = id.GetInt32();

            Assert.True(
                genreId.Value > 0);

            Assert.Equal(
                genreName,
                genre.GetProperty("name").GetString());
        }
        finally
        {
            if (genreId.HasValue)
            {
                await client.DeleteAsync(
                    $"/api/genres/{genreId.Value}");
            }

            await _fixture.DeleteUserAsync(admin);
        }
    }

    [Fact]
    public async Task CreateGenre_WhenUserIsCustomer_ReturnsForbidden()
    {
        // Arrange
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var customer = TestUserData.Customer();

        try
        {
            var loginResponse =
                await auth.RegisterAndLoginAsync(customer);

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
        finally
        {
            await _fixture.DeleteUserAsync(customer);
        }
    }

    [Fact]
    public async Task CreateGenre_WhenUserIsAnonymous_ReturnsUnauthorized()
    {
        // Arrange
        using var client = _fixture.CreateClient();

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
    public async Task UpdateGenre_WhenUserIsAdmin_ReturnsNoContentAndUpdatesGenre()
    {
        // Arrange
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var admin = await _fixture.CreateAdminAsync();

        int? genreId = null;

        try
        {
            var loginResponse =
                await auth.LoginUserAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var originalName =
                $"Genre-{Guid.NewGuid():N}";

            var createResponse =
                await client.PostAsJsonAsync(
                    "/api/genres",
                    new
                    {
                        Name = originalName
                    });

            Assert.Equal(
                HttpStatusCode.Created,
                createResponse.StatusCode);

            var createdGenre =
                await createResponse.Content
                    .ReadFromJsonAsync<JsonElement>();

            genreId =
                createdGenre
                    .GetProperty("id")
                    .GetInt32();

            var updatedName =
                $"Updated-{Guid.NewGuid():N}";

            // Act
            var response =
                await client.PutAsJsonAsync(
                    $"/api/genres/{genreId.Value}",
                    new
                    {
                        Name = updatedName
                    });

            // Assert
            Assert.Equal(
                HttpStatusCode.NoContent,
                response.StatusCode);

            var getResponse =
                await client.GetAsync(
                    $"/api/genres/{genreId.Value}");

            Assert.Equal(
                HttpStatusCode.OK,
                getResponse.StatusCode);

            var updatedGenre =
                await getResponse.Content
                    .ReadFromJsonAsync<JsonElement>();

            Assert.Equal(
                genreId.Value,
                updatedGenre
                    .GetProperty("id")
                    .GetInt32());

            Assert.Equal(
                updatedName,
                updatedGenre
                    .GetProperty("name")
                    .GetString());
        }
        finally
        {
            if (genreId.HasValue)
            {
                await client.DeleteAsync(
                    $"/api/genres/{genreId.Value}");
            }

            await _fixture.DeleteUserAsync(admin);
        }
    }

    [Fact]
    public async Task DeleteGenre_WhenGenreExistsAndUserIsAdmin_ReturnsNoContent()
    {
        // Arrange
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var admin = await _fixture.CreateAdminAsync();

        int? genreId = null;

        try
        {
            var loginResponse =
                await auth.LoginUserAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var genreName =
                $"Genre-ToDelete-{Guid.NewGuid():N}";

            var createResponse =
                await client.PostAsJsonAsync(
                    "/api/genres",
                    new
                    {
                        Name = genreName
                    });

            Assert.Equal(
                HttpStatusCode.Created,
                createResponse.StatusCode);

            var createdGenre =
                await createResponse.Content
                    .ReadFromJsonAsync<JsonElement>();

            genreId =
                createdGenre
                    .GetProperty("id")
                    .GetInt32();

            // Act
            var response =
                await client.DeleteAsync(
                    $"/api/genres/{genreId.Value}");

            // Assert
            Assert.Equal(
                HttpStatusCode.NoContent,
                response.StatusCode);

            var getResponse =
                await client.GetAsync(
                    $"/api/genres/{genreId.Value}");

            Assert.Equal(
                HttpStatusCode.NotFound,
                getResponse.StatusCode);

            genreId = null;
        }
        finally
        {
            if (genreId.HasValue)
            {
                await client.DeleteAsync(
                    $"/api/genres/{genreId.Value}");
            }

            await _fixture.DeleteUserAsync(admin);
        }
    }

    [Fact]
    public async Task DeleteGenre_WhenGenreDoesNotExistAndUserIsAdmin_ReturnsNotFound()
    {
        // Arrange
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var admin = await _fixture.CreateAdminAsync();

        try
        {
            var loginResponse =
                await auth.LoginUserAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var nonExistingId = int.MaxValue;

            // Act
            var response =
                await client.DeleteAsync(
                    $"/api/genres/{nonExistingId}");

            // Assert
            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode);
        }
        finally
        {
            await _fixture.DeleteUserAsync(admin);
        }
    }
}
