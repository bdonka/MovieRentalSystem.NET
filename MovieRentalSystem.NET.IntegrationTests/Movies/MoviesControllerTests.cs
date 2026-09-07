using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Dtos;
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
    public async Task GetMovies_WhenMoviesExist_ReturnsCreatedMovie()
    {
        using var client = _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var admin =
            await _fixture.CreateAdminAsync();

        int? movieId = null;

        try
        {
            var loginResponse =
                await auth.LoginUserAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var createRequest = new
            {
                Title =
                    $"Test Movie {Guid.NewGuid():N}",
                Description =
                    "Movie created for integration test",
                ReleaseYear = 2025,
                RentalPrice = 20m
            };

            var createResponse =
                await client.PostAsJsonAsync(
                    "/api/movies",
                    createRequest);

            Assert.Equal(
                HttpStatusCode.Created,
                createResponse.StatusCode);

            var createdMovie =
                await createResponse.Content
                    .ReadFromJsonAsync<MovieDto>();

            Assert.NotNull(createdMovie);

            movieId = createdMovie.Id;

            using var anonymousClient =
                _fixture.CreateClient();

            var response =
                await anonymousClient.GetAsync(
                    "/api/movies");

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            using var document =
                await response.Content
                    .ReadFromJsonAsync<JsonDocument>();

            Assert.NotNull(document);

            var data =
                document.RootElement
                    .GetProperty("data");

            var returnedMovie =
                data.EnumerateArray()
                    .Single(movie =>
                        movie.GetProperty("id").GetInt32()
                        == movieId.Value);

            Assert.Equal(
                createRequest.Title,
                returnedMovie
                    .GetProperty("title")
                    .GetString());

            Assert.Equal(
                createRequest.Description,
                returnedMovie
                    .GetProperty("description")
                    .GetString());

            Assert.Equal(
                createRequest.ReleaseYear,
                returnedMovie
                    .GetProperty("releaseYear")
                    .GetInt32());

            Assert.Equal(
                createRequest.RentalPrice,
                returnedMovie
                    .GetProperty("rentalPrice")
                    .GetDecimal());
        }
        finally
        {
            if (movieId.HasValue)
            {
                await client.DeleteAsync(
                    $"/api/movies/{movieId.Value}");
            }

            await _fixture.DeleteUserAsync(admin);
        }
    }

    [Fact]
    public async Task GetMovie_WhenMovieExists_ReturnsMovie()
    {
        using var client = _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var admin =
            await _fixture.CreateAdminAsync();

        int? movieId = null;

        try
        {
            var loginResponse =
                await auth.LoginUserAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var createRequest = new
            {
                Title =
                    $"Test Movie {Guid.NewGuid():N}",
                Description =
                    "Movie created for integration test",
                ReleaseYear = 2025,
                RentalPrice = 20m
            };

            var createResponse =
                await client.PostAsJsonAsync(
                    "/api/movies",
                    createRequest);

            Assert.Equal(
                HttpStatusCode.Created,
                createResponse.StatusCode);

            var createdMovie =
                await createResponse.Content
                    .ReadFromJsonAsync<MovieDto>();

            Assert.NotNull(createdMovie);

            movieId = createdMovie.Id;

            var response =
                await client.GetAsync(
                    $"/api/movies/{movieId.Value}");

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            var movie =
                await response.Content
                    .ReadFromJsonAsync<MovieDto>();

            Assert.NotNull(movie);

            Assert.Equal(
                createdMovie.Id,
                movie.Id);

            Assert.Equal(
                createRequest.Title,
                movie.Title);

            Assert.Equal(
                createRequest.Description,
                movie.Description);

            Assert.Equal(
                createRequest.ReleaseYear,
                movie.ReleaseYear);

            Assert.Equal(
                createRequest.RentalPrice,
                movie.RentalPrice);
        }
        finally
        {
            if (movieId.HasValue)
            {
                await client.DeleteAsync(
                    $"/api/movies/{movieId.Value}");
            }

            await _fixture.DeleteUserAsync(admin);
        }
    }

    [Fact]
    public async Task GetMovie_WhenMovieDoesNotExist_ReturnsNotFound()
    {
        using var client = _fixture.CreateClient();

        var response =
            await client.GetAsync(
                $"/api/movies/{int.MaxValue}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateMovie_WhenUserIsAdmin_ReturnsCreatedMovie()
    {
        using var client = _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var admin =
            await _fixture.CreateAdminAsync();

        int? movieId = null;

        try
        {
            var loginResponse =
                await auth.LoginUserAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var request = new
            {
                Title =
                    $"Test Movie {Guid.NewGuid():N}",
                Description =
                    "Integration test movie",
                ReleaseYear = 2025,
                RentalPrice = 20m
            };

            var response =
                await client.PostAsJsonAsync(
                    "/api/movies",
                    request);

            Assert.Equal(
                HttpStatusCode.Created,
                response.StatusCode);

            var movie =
                await response.Content
                    .ReadFromJsonAsync<MovieDto>();

            Assert.NotNull(movie);

            movieId = movie.Id;

            Assert.True(movie.Id > 0);
            Assert.Equal(
                request.Title,
                movie.Title);

            Assert.Equal(
                request.Description,
                movie.Description);

            Assert.Equal(
                request.ReleaseYear,
                movie.ReleaseYear);

            Assert.Equal(
                request.RentalPrice,
                movie.RentalPrice);
        }
        finally
        {
            if (movieId.HasValue)
            {
                await client.DeleteAsync(
                    $"/api/movies/{movieId.Value}");
            }

            await _fixture.DeleteUserAsync(admin);
        }
    }

    [Fact]
    public async Task CreateMovie_WhenUserIsCustomer_ReturnsForbidden()
    {
        using var client = _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var customer =
            TestUserData.Customer();

        try
        {
            var loginResponse =
                await auth.RegisterAndLoginAsync(
                    customer);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var request = new
            {
                Title =
                    $"Customer Movie {Guid.NewGuid():N}",
                Description =
                    "Should not be created",
                ReleaseYear = 2025,
                RentalPrice = 10m
            };

            var response =
                await client.PostAsJsonAsync(
                    "/api/movies",
                    request);

            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode);
        }
        finally
        {
            await _fixture.DeleteUserAsync(
                customer);
        }
    }

    [Fact]
    public async Task CreateMovie_WhenUserIsAnonymous_ReturnsUnauthorized()
    {
        using var client = _fixture.CreateClient();

        var request = new
        {
            Title =
                $"Anonymous Movie {Guid.NewGuid():N}",
            Description =
                "Should not be created",
            ReleaseYear = 2025,
            RentalPrice = 10m
        };

        var response =
            await client.PostAsJsonAsync(
                "/api/movies",
                request);

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task UpdateMovie_WhenMovieExistsAndUserIsAdmin_ReturnsNoContentAndUpdatesMovie()
    {
        using var client = _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var admin =
            await _fixture.CreateAdminAsync();

        int? movieId = null;

        try
        {
            var loginResponse =
                await auth.LoginUserAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var createRequest = new
            {
                Title =
                    $"Original Movie {Guid.NewGuid():N}",
                Description =
                    "Original description",
                ReleaseYear = 2025,
                RentalPrice = 20m
            };

            var createResponse =
                await client.PostAsJsonAsync(
                    "/api/movies",
                    createRequest);

            Assert.Equal(
                HttpStatusCode.Created,
                createResponse.StatusCode);

            var createdMovie =
                await createResponse.Content
                    .ReadFromJsonAsync<MovieDto>();

            Assert.NotNull(createdMovie);

            movieId = createdMovie.Id;

            var updateRequest = new
            {
                Title =
                    $"Updated Movie {Guid.NewGuid():N}",
                Description =
                    "Updated description",
                ReleaseYear = 2026,
                RentalPrice = 30m
            };

            var response =
                await client.PutAsJsonAsync(
                    $"/api/movies/{movieId.Value}",
                    updateRequest);

            Assert.Equal(
                HttpStatusCode.NoContent,
                response.StatusCode);

            var getResponse =
                await client.GetAsync(
                    $"/api/movies/{movieId.Value}");

            Assert.Equal(
                HttpStatusCode.OK,
                getResponse.StatusCode);

            var updatedMovie =
                await getResponse.Content
                    .ReadFromJsonAsync<MovieDto>();

            Assert.NotNull(updatedMovie);

            Assert.Equal(
                movieId.Value,
                updatedMovie.Id);

            Assert.Equal(
                updateRequest.Title,
                updatedMovie.Title);

            Assert.Equal(
                updateRequest.Description,
                updatedMovie.Description);

            Assert.Equal(
                updateRequest.ReleaseYear,
                updatedMovie.ReleaseYear);

            Assert.Equal(
                updateRequest.RentalPrice,
                updatedMovie.RentalPrice);
        }
        finally
        {
            if (movieId.HasValue)
            {
                await client.DeleteAsync(
                    $"/api/movies/{movieId.Value}");
            }

            await _fixture.DeleteUserAsync(admin);
        }
    }

    [Fact]
    public async Task DeleteMovie_WhenMovieExistsAndUserIsAdmin_ReturnsNoContentAndRemovesMovie()
    {
        using var client = _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var admin =
            await _fixture.CreateAdminAsync();

        int? movieId = null;

        try
        {
            var loginResponse =
                await auth.LoginUserAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var createRequest = new
            {
                Title =
                    $"Movie To Delete {Guid.NewGuid():N}",
                Description =
                    "Movie created for delete test",
                ReleaseYear = 2025,
                RentalPrice = 20m
            };

            var createResponse =
                await client.PostAsJsonAsync(
                    "/api/movies",
                    createRequest);

            Assert.Equal(
                HttpStatusCode.Created,
                createResponse.StatusCode);

            var createdMovie =
                await createResponse.Content
                    .ReadFromJsonAsync<MovieDto>();

            Assert.NotNull(createdMovie);

            movieId = createdMovie.Id;

            var response =
                await client.DeleteAsync(
                    $"/api/movies/{movieId.Value}");

            Assert.Equal(
                HttpStatusCode.NoContent,
                response.StatusCode);

            var getResponse =
                await client.GetAsync(
                    $"/api/movies/{movieId.Value}");

            Assert.Equal(
                HttpStatusCode.NotFound,
                getResponse.StatusCode);

            movieId = null;
        }
        finally
        {
            if (movieId.HasValue)
            {
                await client.DeleteAsync(
                    $"/api/movies/{movieId.Value}");
            }

            await _fixture.DeleteUserAsync(admin);
        }
    }

    [Fact]
    public async Task DeleteMovie_WhenMovieDoesNotExistAndUserIsAdmin_ReturnsNotFound()
    {
        using var client = _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var admin =
            await _fixture.CreateAdminAsync();

        try
        {
            var loginResponse =
                await auth.LoginUserAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var response =
                await client.DeleteAsync(
                    $"/api/movies/{int.MaxValue}");

            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode);
        }
        finally
        {
            await _fixture.DeleteUserAsync(admin);
        }
    }

    [Fact]
    public async Task CreateMovie_WhenRequestDataIsInvalidAndUserIsAdmin_ReturnsBadRequest()
    {
        using var client = _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var admin =
            await _fixture.CreateAdminAsync();

        try
        {
            var loginResponse =
                await auth.LoginUserAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var request = new
            {
                Title = string.Empty,
                Description = string.Empty,
                ReleaseYear = 0,
                RentalPrice = -1m
            };

            var response =
                await client.PostAsJsonAsync(
                    "/api/movies",
                    request);

            Assert.Equal(
                HttpStatusCode.BadRequest,
                response.StatusCode);
        }
        finally
        {
            await _fixture.DeleteUserAsync(admin);
        }
    }
}
