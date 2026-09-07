using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Enums;
using MovieRentalSystem.NET.IntegrationTests.Fixtures;
using MovieRentalSystem.NET.IntegrationTests.Helpers;

namespace MovieRentalSystem.NET.IntegrationTests.MoviePhysicalCopies;

[Collection("Aspire collection")]
public class MoviePhysicalCopiesControllerTests
{
    private readonly AspireFixture _fixture;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public MoviePhysicalCopiesControllerTests(AspireFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetMoviePhysicalCopies_WhenUserIsAdmin_ReturnsOkWithCreatedCopy()
    {
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var admin = await _fixture.CreateAdminAsync();

        int? movieId = null;
        int? copyId = null;

        try
        {
            await LoginAsAdminAsync(auth, admin);

            var movie = await CreateMovieAsync(client);
            movieId = movie.Id;

            var copy = await CreateMoviePhysicalCopyAsync(
                client,
                movie.Id,
                "GET-LIST");

            copyId = copy.Id;

            var response =
                await client.GetAsync("/api/moviePhysicalCopies");

            var result =
                await ReadResponseAsync<
                    PagedResponse<MoviePhysicalCopyDto>>(
                    response);

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            Assert.NotNull(result);
            Assert.NotNull(result.Data);

            var returnedCopy =
                result.Data.Single(
                    item => item.Id == copy.Id);

            Assert.Equal(copy.Id, returnedCopy.Id);
            Assert.Equal(movie.Id, returnedCopy.MovieId);
            Assert.Equal(copy.Code, returnedCopy.Code);
            Assert.Equal(copy.Status, returnedCopy.Status);
        }
        finally
        {
            await CleanupAsync(
                client,
                copyId,
                movieId);

            await _fixture.DeleteUserAsync(admin);
        }
    }

    [Fact]
    public async Task GetMoviePhysicalCopies_WhenUserIsWorker_ReturnsOkWithCreatedCopy()
    {
        using var workerClient = _fixture.CreateClient();
        using var adminClient = _fixture.CreateClient();

        var workerAuth = _fixture.CreateAuthHelper(workerClient);
        var adminAuth = _fixture.CreateAuthHelper(adminClient);

        var worker = await _fixture.CreateWorkerAsync();
        var admin = await _fixture.CreateAdminAsync();

        int? movieId = null;
        int? copyId = null;

        try
        {
            await LoginAsWorkerAsync(workerAuth, worker);
            await LoginAsAdminAsync(adminAuth, admin);

            var movie =
                await CreateMovieAsync(adminClient);

            movieId = movie.Id;

            var copy =
                await CreateMoviePhysicalCopyAsync(
                    adminClient,
                    movie.Id,
                    "GET-WORKER");

            copyId = copy.Id;

            var response =
                await workerClient.GetAsync(
                    "/api/moviePhysicalCopies");

            var result =
                await ReadResponseAsync<
                    PagedResponse<MoviePhysicalCopyDto>>(
                    response);

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            Assert.NotNull(result);
            Assert.NotNull(result.Data);

            var returnedCopy =
                result.Data.Single(
                    item => item.Id == copy.Id);

            Assert.Equal(copy.Id, returnedCopy.Id);
            Assert.Equal(movie.Id, returnedCopy.MovieId);
            Assert.Equal(copy.Code, returnedCopy.Code);
            Assert.Equal(copy.Status, returnedCopy.Status);
        }
        finally
        {
            await CleanupAsync(
                adminClient,
                copyId,
                movieId);

            await _fixture.DeleteUserAsync(admin);
            await _fixture.DeleteUserAsync(worker);
        }
    }

    [Fact]
    public async Task GetMoviePhysicalCopies_WhenUserIsCustomer_ReturnsForbidden()
    {
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var customer = TestUserData.Customer();

        try
        {
            await RegisterAndLoginAsync(
                auth,
                customer);

            var response =
                await client.GetAsync(
                    "/api/moviePhysicalCopies");

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
    public async Task GetMoviePhysicalCopies_WhenUserIsAnonymous_ReturnsUnauthorized()
    {
        using var client = _fixture.CreateClient();

        var response =
            await client.GetAsync(
                "/api/moviePhysicalCopies");

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task GetMoviePhysicalCopy_WhenCopyExistsAndUserIsAdmin_ReturnsOkWithCopy()
    {
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var admin = await _fixture.CreateAdminAsync();

        int? movieId = null;
        int? copyId = null;

        try
        {
            await LoginAsAdminAsync(auth, admin);

            var movie =
                await CreateMovieAsync(client);

            movieId = movie.Id;

            var copy =
                await CreateMoviePhysicalCopyAsync(
                    client,
                    movie.Id,
                    "GET-SINGLE");

            copyId = copy.Id;

            var response =
                await client.GetAsync(
                    $"/api/moviePhysicalCopies/{copy.Id}");

            var returnedCopy =
                await ReadResponseAsync<MoviePhysicalCopyDto>(
                    response);

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            Assert.NotNull(returnedCopy);

            Assert.Equal(copy.Id, returnedCopy.Id);
            Assert.Equal(movie.Id, returnedCopy.MovieId);
            Assert.Equal(copy.Code, returnedCopy.Code);
            Assert.Equal(
                MovieCopyStatus.Available,
                returnedCopy.Status);
        }
        finally
        {
            await CleanupAsync(
                client,
                copyId,
                movieId);

            await _fixture.DeleteUserAsync(admin);
        }
    }

    [Fact]
    public async Task GetMoviePhysicalCopy_WhenCopyDoesNotExistAndUserIsAdmin_ReturnsNotFound()
    {
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var admin = await _fixture.CreateAdminAsync();

        try
        {
            await LoginAsAdminAsync(auth, admin);

            var response =
                await client.GetAsync(
                    $"/api/moviePhysicalCopies/{int.MaxValue}");

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
    public async Task GetMoviePhysicalCopy_WhenUserIsCustomer_ReturnsForbidden()
    {
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var customer = TestUserData.Customer();

        try
        {
            await RegisterAndLoginAsync(
                auth,
                customer);

            var response =
                await client.GetAsync(
                    $"/api/moviePhysicalCopies/{int.MaxValue}");

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
    public async Task GetMoviePhysicalCopy_WhenUserIsAnonymous_ReturnsUnauthorized()
    {
        using var client = _fixture.CreateClient();

        var response =
            await client.GetAsync(
                $"/api/moviePhysicalCopies/{int.MaxValue}");

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateMoviePhysicalCopy_WhenUserIsAdmin_ReturnsCreatedWithCreatedCopy()
    {
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var admin = await _fixture.CreateAdminAsync();

        int? movieId = null;
        int? copyId = null;

        try
        {
            await LoginAsAdminAsync(auth, admin);

            var movie =
                await CreateMovieAsync(client);

            movieId = movie.Id;

            var copy =
                await CreateMoviePhysicalCopyAsync(
                    client,
                    movie.Id,
                    "CREATE-COPY");

            copyId = copy.Id;

            Assert.True(copy.Id > 0);
            Assert.Equal(movie.Id, copy.MovieId);
            Assert.Equal(
                $"CREATE-COPY-{movie.Id}",
                copy.Code);
            Assert.Equal(
                MovieCopyStatus.Available,
                copy.Status);
        }
        finally
        {
            await CleanupAsync(
                client,
                copyId,
                movieId);

            await _fixture.DeleteUserAsync(admin);
        }
    }

    [Fact]
    public async Task CreateMoviePhysicalCopy_WhenUserIsCustomer_ReturnsForbidden()
    {
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var customer = TestUserData.Customer();

        try
        {
            await RegisterAndLoginAsync(
                auth,
                customer);

            var request = new
            {
                MovieId = int.MaxValue,
                Code = $"CUSTOMER-COPY-{Guid.NewGuid():N}"
            };

            var response =
                await client.PostAsJsonAsync(
                    "/api/moviePhysicalCopies",
                    request);

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
    public async Task CreateMoviePhysicalCopy_WhenUserIsAnonymous_ReturnsUnauthorized()
    {
        using var client = _fixture.CreateClient();

        var request = new
        {
            MovieId = int.MaxValue,
            Code = $"ANONYMOUS-COPY-{Guid.NewGuid():N}"
        };

        var response =
            await client.PostAsJsonAsync(
                "/api/moviePhysicalCopies",
                request);

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateMoviePhysicalCopy_WhenMovieDoesNotExistAndUserIsAdmin_ReturnsNotFound()
    {
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var admin = await _fixture.CreateAdminAsync();

        try
        {
            await LoginAsAdminAsync(auth, admin);

            var request = new
            {
                MovieId = int.MaxValue,
                Code = $"INVALID-MOVIE-{Guid.NewGuid():N}"
            };

            var response =
                await client.PostAsJsonAsync(
                    "/api/moviePhysicalCopies",
                    request);

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
    public async Task UpdateMoviePhysicalCopy_WhenCopyExistsAndUserIsAdmin_ReturnsNoContentAndUpdatesStatus()
    {
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var admin = await _fixture.CreateAdminAsync();

        int? movieId = null;
        int? copyId = null;

        try
        {
            await LoginAsAdminAsync(auth, admin);

            var movie =
                await CreateMovieAsync(client);

            movieId = movie.Id;

            var copy =
                await CreateMoviePhysicalCopyAsync(
                    client,
                    movie.Id,
                    "UPDATE-COPY");

            copyId = copy.Id;

            var request = new
            {
                Status = MovieCopyStatus.Rented
            };

            var response =
                await client.PutAsJsonAsync(
                    $"/api/moviePhysicalCopies/{copy.Id}/{movie.Id}",
                    request);

            Assert.Equal(
                HttpStatusCode.NoContent,
                response.StatusCode);

            var getResponse =
                await client.GetAsync(
                    $"/api/moviePhysicalCopies/{copy.Id}");

            var updatedCopy =
                await ReadResponseAsync<MoviePhysicalCopyDto>(
                    getResponse);

            Assert.Equal(
                HttpStatusCode.OK,
                getResponse.StatusCode);

            Assert.NotNull(updatedCopy);

            Assert.Equal(copy.Id, updatedCopy.Id);
            Assert.Equal(movie.Id, updatedCopy.MovieId);
            Assert.Equal(copy.Code, updatedCopy.Code);
            Assert.Equal(
                MovieCopyStatus.Rented,
                updatedCopy.Status);
        }
        finally
        {
            await CleanupAsync(
                client,
                copyId,
                movieId);

            await _fixture.DeleteUserAsync(admin);
        }
    }

    [Fact]
    public async Task UpdateMoviePhysicalCopy_WhenCopyDoesNotExistAndUserIsAdmin_ReturnsNotFound()
    {
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var admin = await _fixture.CreateAdminAsync();

        try
        {
            await LoginAsAdminAsync(auth, admin);

            var request = new
            {
                Status = MovieCopyStatus.Rented
            };

            var response =
                await client.PutAsJsonAsync(
                    $"/api/moviePhysicalCopies/{int.MaxValue}/{int.MaxValue}",
                    request);

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
    public async Task UpdateMoviePhysicalCopy_WhenUserIsCustomer_ReturnsForbidden()
    {
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var customer = TestUserData.Customer();

        try
        {
            await RegisterAndLoginAsync(
                auth,
                customer);

            var request = new
            {
                Status = MovieCopyStatus.Rented
            };

            var response =
                await client.PutAsJsonAsync(
                    $"/api/moviePhysicalCopies/{int.MaxValue}/{int.MaxValue}",
                    request);

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
    public async Task UpdateMoviePhysicalCopy_WhenUserIsAnonymous_ReturnsUnauthorized()
    {
        using var client = _fixture.CreateClient();

        var request = new
        {
            Status = MovieCopyStatus.Rented
        };

        var response =
            await client.PutAsJsonAsync(
                $"/api/moviePhysicalCopies/{int.MaxValue}/{int.MaxValue}",
                request);

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task DeleteMoviePhysicalCopy_WhenCopyExistsAndUserIsAdmin_ReturnsNoContentAndDeletesCopy()
    {
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var admin = await _fixture.CreateAdminAsync();

        int? movieId = null;
        int? copyId = null;

        try
        {
            await LoginAsAdminAsync(auth, admin);

            var movie =
                await CreateMovieAsync(client);

            movieId = movie.Id;

            var copy =
                await CreateMoviePhysicalCopyAsync(
                    client,
                    movie.Id,
                    "DELETE-COPY");

            copyId = copy.Id;

            var response =
                await client.DeleteAsync(
                    $"/api/moviePhysicalCopies/{copy.Id}/{movie.Id}");

            Assert.Equal(
                HttpStatusCode.NoContent,
                response.StatusCode);

            var getResponse =
                await client.GetAsync(
                    $"/api/moviePhysicalCopies/{copy.Id}");

            Assert.Equal(
                HttpStatusCode.NotFound,
                getResponse.StatusCode);

            // Copy has already been deleted successfully.
            copyId = null;
        }
        finally
        {
            await CleanupAsync(
                client,
                copyId,
                movieId);

            await _fixture.DeleteUserAsync(admin);
        }
    }

    [Fact]
    public async Task DeleteMoviePhysicalCopy_WhenCopyDoesNotExistAndUserIsAdmin_ReturnsNotFound()
    {
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var admin = await _fixture.CreateAdminAsync();

        try
        {
            await LoginAsAdminAsync(auth, admin);

            var response =
                await client.DeleteAsync(
                    $"/api/moviePhysicalCopies/{int.MaxValue}/{int.MaxValue}");

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
    public async Task DeleteMoviePhysicalCopy_WhenUserIsCustomer_ReturnsForbidden()
    {
        using var client = _fixture.CreateClient();

        var auth = _fixture.CreateAuthHelper(client);
        var customer = TestUserData.Customer();

        try
        {
            await RegisterAndLoginAsync(
                auth,
                customer);

            var response =
                await client.DeleteAsync(
                    $"/api/moviePhysicalCopies/{int.MaxValue}/{int.MaxValue}");

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
    public async Task DeleteMoviePhysicalCopy_WhenUserIsAnonymous_ReturnsUnauthorized()
    {
        using var client = _fixture.CreateClient();

        var response =
            await client.DeleteAsync(
                $"/api/moviePhysicalCopies/{int.MaxValue}/{int.MaxValue}");

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    private static async Task LoginAsAdminAsync(
        AuthTestHelper auth,
        TestUserData admin)
    {
        var response =
            await auth.LoginUserAsync(admin);

        await AssertSuccessfulAuthenticationAsync(response);
    }

    private static async Task LoginAsWorkerAsync(
        AuthTestHelper auth,
        TestUserData worker)
    {
        var response =
            await auth.LoginUserAsync(worker);

        await AssertSuccessfulAuthenticationAsync(response);
    }

    private static async Task RegisterAndLoginAsync(
        AuthTestHelper auth,
        TestUserData user)
    {
        var response =
            await auth.RegisterAndLoginAsync(user);

        await AssertSuccessfulAuthenticationAsync(response);
    }

    private static async Task AssertSuccessfulAuthenticationAsync(
        HttpResponseMessage response)
    {
        var body =
            await response.Content.ReadAsStringAsync();

        Assert.True(
            response.IsSuccessStatusCode,
            $"Authentication failed. " +
            $"Status: {response.StatusCode}. " +
            $"Body: {body}");
    }

    private static async Task<T?> ReadResponseAsync<T>(
        HttpResponseMessage response)
    {
        return await response.Content.ReadFromJsonAsync<T>(
            JsonOptions);
    }

    private static async Task<MovieDto> CreateMovieAsync(
        HttpClient client)
    {
        var request = new
        {
            Title = $"Test Movie {Guid.NewGuid():N}",
            Description = "Movie created for integration test",
            ReleaseYear = 2025,
            RentalPrice = 20m
        };

        var response =
            await client.PostAsJsonAsync(
                "/api/movies",
                request);

        var body =
            await response.Content.ReadAsStringAsync();

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var movie =
            await response.Content.ReadFromJsonAsync<MovieDto>();

        Assert.NotNull(movie);

        Assert.True(
            movie.Id > 0);

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

        return movie;
    }

    private static async Task<MoviePhysicalCopyDto>
        CreateMoviePhysicalCopyAsync(
            HttpClient client,
            int movieId,
            string codePrefix)
    {
        var request = new
        {
            MovieId = movieId,
            Code = $"{codePrefix}-{movieId}"
        };

        var response =
            await client.PostAsJsonAsync(
                "/api/moviePhysicalCopies",
                request);

        var responseBody =
            await response.Content.ReadAsStringAsync();

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var copy =
            await response.Content.ReadFromJsonAsync<
                MoviePhysicalCopyDto>(
                JsonOptions);

        Assert.NotNull(copy);

        Assert.True(
            copy.Id > 0);

        Assert.Equal(
            request.MovieId,
            copy.MovieId);

        Assert.Equal(
            request.Code,
            copy.Code);

        Assert.Equal(
            MovieCopyStatus.Available,
            copy.Status);

        return copy;
    }

    private static async Task CleanupAsync(
    HttpClient client,
    int? copyId,
    int? movieId)
    {
        if (copyId.HasValue)
        {
            var deleteCopyResponse =
                await client.DeleteAsync(
                    $"/api/moviePhysicalCopies/{copyId.Value}/{movieId}");

            var deleteCopyBody =
                await deleteCopyResponse.Content.ReadAsStringAsync();

            Console.WriteLine(
                $"DELETE COPY {copyId}: " +
                $"{(int) deleteCopyResponse.StatusCode} " +
                $"{deleteCopyBody}");

            Assert.Equal(
                HttpStatusCode.NoContent,
                deleteCopyResponse.StatusCode);

            var verifyCopyResponse =
                await client.GetAsync(
                    $"/api/moviePhysicalCopies/{copyId.Value}");

            var verifyCopyBody =
                await verifyCopyResponse.Content.ReadAsStringAsync();

            Console.WriteLine(
                $"GET COPY {copyId} AFTER DELETE: " +
                $"{(int) verifyCopyResponse.StatusCode} " +
                $"{verifyCopyBody}");

            Assert.Equal(
                HttpStatusCode.NotFound,
                verifyCopyResponse.StatusCode);
        }

        if (movieId.HasValue)
        {
            Console.WriteLine(
                $"ABOUT TO DELETE MOVIE {movieId}");

            var deleteMovieResponse =
                await client.DeleteAsync(
                    $"/api/movies/{movieId.Value}");

            var deleteMovieBody =
                await deleteMovieResponse.Content.ReadAsStringAsync();

            Console.WriteLine(
                $"DELETE MOVIE {movieId}: " +
                $"{(int) deleteMovieResponse.StatusCode} " +
                $"{deleteMovieBody}");

            Assert.Equal(
                HttpStatusCode.NoContent,
                deleteMovieResponse.StatusCode);
        }
    }
}
