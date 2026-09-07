using System.Net;
using System.Net.Http.Json;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Domain.Enums;
using MovieRentalSystem.NET.IntegrationTests.Fixtures;
using MovieRentalSystem.NET.IntegrationTests.Helpers;

namespace MovieRentalSystem.NET.IntegrationTests.Rentals;

[Collection("Aspire collection")]
public class RentalsControllerTests
{
    private readonly AspireFixture _fixture;

    public RentalsControllerTests(AspireFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetRentals_WhenUserIsAdmin_ReturnsOkWithRentals()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var admin =
            TestUserData.Admin();

        try
        {
            var loginResponse =
                await auth.RegisterAndLoginAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            // Act
            var response =
                await client.GetAsync(
                    "/api/rentals");

            // Assert
            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            var result =
                await response.Content
                    .ReadFromJsonAsync<PagedResponse<RentalDto>>();

            Assert.NotNull(result);
            Assert.NotNull(result.Data);
        }
        finally
        {
            await auth.DeleteCurrentUserAsync();
        }
    }

    [Fact]
    public async Task GetRentals_WhenUserIsCustomer_ReturnsForbidden()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var customer =
            TestUserData.Customer();

        try
        {
            var loginResponse =
                await auth.RegisterAndLoginAsync(customer);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            // Act
            var response =
                await client.GetAsync(
                    "/api/rentals");

            // Assert
            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode);
        }
        finally
        {
            await auth.DeleteCurrentUserAsync();
        }
    }

    [Fact]
    public async Task GetRentals_WhenUserIsAnonymous_ReturnsUnauthorized()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        // Act
        var response =
            await client.GetAsync(
                "/api/rentals");

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task GetRentalById_WhenRentalExistsAndUserIsAdmin_ReturnsOkWithRental()
    {
        // Arrange
        using var customerClient =
            _fixture.CreateClient();

        var customerAuth =
            _fixture.CreateAuthHelper(customerClient);

        var customer =
            TestUserData.Customer();

        var customerLoginResponse =
            await customerAuth.RegisterAndLoginAsync(customer);

        Assert.True(
            customerLoginResponse.IsSuccessStatusCode);

        var currentUser =
            await customerAuth.GetCurrentUserAsync();

        Assert.NotNull(currentUser);

        var rentalStartDate =
            DateTime.UtcNow;

        var dueDate =
            rentalStartDate.AddDays(7);

        var createRentalRequest = new
        {
            UserId = currentUser.Id,
            MoviePhysicalCopyId = 1,
            RentalStartDate = rentalStartDate,
            DueDate = dueDate
        };

        var createResponse =
            await customerClient.PostAsJsonAsync(
                "/api/rentals",
                createRentalRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            createResponse.StatusCode);

        var createdRental =
            await createResponse.Content
                .ReadFromJsonAsync<RentalDto>();

        Assert.NotNull(createdRental);

        using var adminClient =
            _fixture.CreateClient();

        var adminAuth =
            _fixture.CreateAuthHelper(adminClient);

        var admin =
            TestUserData.Admin();

        try
        {
            var adminLoginResponse =
                await adminAuth.RegisterAndLoginAsync(admin);

            Assert.True(
                adminLoginResponse.IsSuccessStatusCode);

            // Act
            var response =
                await adminClient.GetAsync(
                    $"/api/rentals/{createdRental.Id}");

            // Assert
            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            var rental =
                await response.Content
                    .ReadFromJsonAsync<RentalDto>();

            Assert.NotNull(rental);

            Assert.Equal(
                createdRental.Id,
                rental.Id);

            Assert.Equal(
                createdRental.UserId,
                rental.UserId);

            Assert.Equal(
                createdRental.MoviePhysicalCopyId,
                rental.MoviePhysicalCopyId);
        }
        finally
        {
            await adminClient.DeleteAsync(
                $"/api/rentals/{createdRental.Id}");

            await adminAuth.DeleteCurrentUserAsync();
            await customerAuth.DeleteCurrentUserAsync();
        }
    }

    [Fact]
    public async Task GetRentalById_WhenRentalDoesNotExistAndUserIsAdmin_ReturnsNotFound()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var admin =
            TestUserData.Admin();

        try
        {
            var loginResponse =
                await auth.RegisterAndLoginAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            // Act
            var response =
                await client.GetAsync(
                    $"/api/rentals/{int.MaxValue}");

            // Assert
            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode);
        }
        finally
        {
            await auth.DeleteCurrentUserAsync();
        }
    }

    [Fact]
    public async Task GetRentalById_WhenUserIsAnonymous_ReturnsUnauthorized()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        // Act
        var response =
            await client.GetAsync(
                $"/api/rentals/{int.MaxValue}");

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task GetRentalById_WhenUserIsCustomer_ReturnsForbidden()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var customer =
            TestUserData.Customer();

        try
        {
            var loginResponse =
                await auth.RegisterAndLoginAsync(customer);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            // Act
            var response =
                await client.GetAsync(
                    $"/api/rentals/{int.MaxValue}");

            // Assert
            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode);
        }
        finally
        {
            await auth.DeleteCurrentUserAsync();
        }
    }

    [Fact]
    public async Task CreateRental_WhenUserIsAuthenticated_ReturnsCreatedRental()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var customer =
            TestUserData.Customer();

        try
        {
            var loginResponse =
                await auth.RegisterAndLoginAsync(customer);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var currentUser =
                await auth.GetCurrentUserAsync();

            Assert.NotNull(currentUser);

            var rentalStartDate =
                DateTime.UtcNow;

            var dueDate =
                rentalStartDate.AddDays(7);

            var request = new
            {
                UserId = currentUser.Id,
                MoviePhysicalCopyId = 1,
                RentalStartDate = rentalStartDate,
                DueDate = dueDate
            };

            // Act
            var response =
                await client.PostAsJsonAsync(
                    "/api/rentals",
                    request);

            // Assert
            Assert.Equal(
                HttpStatusCode.Created,
                response.StatusCode);

            var createdRental =
                await response.Content
                    .ReadFromJsonAsync<RentalDto>();

            Assert.NotNull(createdRental);

            Assert.True(
                createdRental.Id > 0);

            Assert.Equal(
                request.UserId,
                createdRental.UserId);

            Assert.Equal(
                request.MoviePhysicalCopyId,
                createdRental.MoviePhysicalCopyId);
        }
        finally
        {
            await auth.DeleteCurrentUserAsync();
        }
    }

    [Fact]
    public async Task CreateRental_WhenUserIsAnonymous_ReturnsUnauthorized()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var request = new
        {
            UserId = Guid.NewGuid().ToString(),
            MoviePhysicalCopyId = 1,
            RentalStartDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        // Act
        var response =
            await client.PostAsJsonAsync(
                "/api/rentals",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateRental_WhenMoviePhysicalCopyDoesNotExist_ReturnsBadRequest()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var customer =
            TestUserData.Customer();

        try
        {
            var loginResponse =
                await auth.RegisterAndLoginAsync(customer);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var currentUser =
                await auth.GetCurrentUserAsync();

            Assert.NotNull(currentUser);

            var rentalStartDate =
                DateTime.UtcNow;

            var request = new
            {
                UserId = currentUser.Id,
                MoviePhysicalCopyId = int.MaxValue,
                RentalStartDate = rentalStartDate,
                DueDate = rentalStartDate.AddDays(7)
            };

            // Act
            var response =
                await client.PostAsJsonAsync(
                    "/api/rentals",
                    request);

            // Assert
            Assert.Equal(
                HttpStatusCode.BadRequest,
                response.StatusCode);
        }
        finally
        {
            await auth.DeleteCurrentUserAsync();
        }
    }

    [Fact]
    public async Task UpdateRental_WhenRentalExistsAndUserIsAdmin_ReturnsNoContentAndUpdatesRental()
    {
        // Arrange
        using var customerClient =
            _fixture.CreateClient();

        var customerAuth =
            _fixture.CreateAuthHelper(customerClient);

        var customer =
            TestUserData.Customer();

        var customerLoginResponse =
            await customerAuth.RegisterAndLoginAsync(customer);

        Assert.True(
            customerLoginResponse.IsSuccessStatusCode);

        var currentUser =
            await customerAuth.GetCurrentUserAsync();

        Assert.NotNull(currentUser);

        var rentalStartDate =
            DateTime.UtcNow;

        var createRequest = new
        {
            UserId = currentUser.Id,
            MoviePhysicalCopyId = 1,
            RentalStartDate = rentalStartDate,
            DueDate = rentalStartDate.AddDays(7)
        };

        var createResponse =
            await customerClient.PostAsJsonAsync(
                "/api/rentals",
                createRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            createResponse.StatusCode);

        var createdRental =
            await createResponse.Content
                .ReadFromJsonAsync<RentalDto>();

        Assert.NotNull(createdRental);

        using var adminClient =
            _fixture.CreateClient();

        var adminAuth =
            _fixture.CreateAuthHelper(adminClient);

        var admin =
            TestUserData.Admin();

        try
        {
            var adminLoginResponse =
                await adminAuth.RegisterAndLoginAsync(admin);

            Assert.True(
                adminLoginResponse.IsSuccessStatusCode);

            var updatedStartDate =
                DateTime.UtcNow.AddDays(1);

            var updatedDueDate =
                updatedStartDate.AddDays(10);

            var updateRequest = new
            {
                RentalStartDate = updatedStartDate,
                DueDate = updatedDueDate,
                ReturnDate = (DateTime?) null,
                TotalPrice = 50m,
                Status = RentalStatus.Active
            };

            // Act
            var response =
                await adminClient.PutAsJsonAsync(
                    $"/api/rentals/{createdRental.Id}",
                    updateRequest);

            // Assert
            Assert.Equal(
                HttpStatusCode.NoContent,
                response.StatusCode);

            var getResponse =
                await adminClient.GetAsync(
                    $"/api/rentals/{createdRental.Id}");

            Assert.Equal(
                HttpStatusCode.OK,
                getResponse.StatusCode);

            var updatedRental =
                await getResponse.Content
                    .ReadFromJsonAsync<RentalDto>();

            Assert.NotNull(updatedRental);

            Assert.Equal(
                createdRental.Id,
                updatedRental.Id);

            Assert.Equal(
                updateRequest.RentalStartDate,
                updatedRental.RentalStartDate);

            Assert.Equal(
                updateRequest.DueDate,
                updatedRental.DueDate);

            Assert.Equal(
                updateRequest.ReturnDate,
                updatedRental.ReturnDate);

            Assert.Equal(
                updateRequest.TotalPrice,
                updatedRental.TotalPrice);

            Assert.Equal(
                updateRequest.Status,
                updatedRental.Status);
        }
        finally
        {
            await adminClient.DeleteAsync(
                $"/api/rentals/{createdRental.Id}");

            await adminAuth.DeleteCurrentUserAsync();
            await customerAuth.DeleteCurrentUserAsync();
        }
    }

    [Fact]
    public async Task UpdateRental_WhenRentalDoesNotExistAndUserIsAdmin_ReturnsNotFound()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var admin =
            TestUserData.Admin();

        try
        {
            var loginResponse =
                await auth.RegisterAndLoginAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var startDate =
                DateTime.UtcNow;

            var request = new
            {
                RentalStartDate = startDate,
                DueDate = startDate.AddDays(10),
                ReturnDate = (DateTime?) null,
                TotalPrice = 50m,
                Status = RentalStatus.Active
            };

            // Act
            var response =
                await client.PutAsJsonAsync(
                    $"/api/rentals/{int.MaxValue}",
                    request);

            // Assert
            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode);
        }
        finally
        {
            await auth.DeleteCurrentUserAsync();
        }
    }

    [Fact]
    public async Task UpdateRental_WhenUserIsCustomer_ReturnsForbidden()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var customer =
            TestUserData.Customer();

        try
        {
            var loginResponse =
                await auth.RegisterAndLoginAsync(customer);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            var startDate =
                DateTime.UtcNow;

            var request = new
            {
                RentalStartDate = startDate,
                DueDate = startDate.AddDays(10),
                ReturnDate = (DateTime?) null,
                TotalPrice = 50m,
                Status = RentalStatus.Active
            };

            // Act
            var response =
                await client.PutAsJsonAsync(
                    $"/api/rentals/{int.MaxValue}",
                    request);

            // Assert
            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode);
        }
        finally
        {
            await auth.DeleteCurrentUserAsync();
        }
    }

    [Fact]
    public async Task UpdateRental_WhenUserIsAnonymous_ReturnsUnauthorized()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var startDate =
            DateTime.UtcNow;

        var request = new
        {
            RentalStartDate = startDate,
            DueDate = startDate.AddDays(10),
            ReturnDate = (DateTime?) null,
            TotalPrice = 50m,
            Status = RentalStatus.Active
        };

        // Act
        var response =
            await client.PutAsJsonAsync(
                $"/api/rentals/{int.MaxValue}",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task DeleteRental_WhenRentalExistsAndUserIsAdmin_ReturnsNoContentAndRentalIsNotFound()
    {
        // Arrange
        using var customerClient =
            _fixture.CreateClient();

        var customerAuth =
            _fixture.CreateAuthHelper(customerClient);

        var customer =
            TestUserData.Customer();

        var customerLoginResponse =
            await customerAuth.RegisterAndLoginAsync(customer);

        Assert.True(
            customerLoginResponse.IsSuccessStatusCode);

        var currentUser =
            await customerAuth.GetCurrentUserAsync();

        Assert.NotNull(currentUser);

        var rentalStartDate =
            DateTime.UtcNow;

        var createRequest = new
        {
            UserId = currentUser.Id,
            MoviePhysicalCopyId = 1,
            RentalStartDate = rentalStartDate,
            DueDate = rentalStartDate.AddDays(7)
        };

        var createResponse =
            await customerClient.PostAsJsonAsync(
                "/api/rentals",
                createRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            createResponse.StatusCode);

        var createdRental =
            await createResponse.Content
                .ReadFromJsonAsync<RentalDto>();

        Assert.NotNull(createdRental);

        using var adminClient =
            _fixture.CreateClient();

        var adminAuth =
            _fixture.CreateAuthHelper(adminClient);

        var admin =
            TestUserData.Admin();

        try
        {
            var adminLoginResponse =
                await adminAuth.RegisterAndLoginAsync(admin);

            Assert.True(
                adminLoginResponse.IsSuccessStatusCode);

            // Act
            var response =
                await adminClient.DeleteAsync(
                    $"/api/rentals/{createdRental.Id}");

            // Assert
            Assert.Equal(
                HttpStatusCode.NoContent,
                response.StatusCode);

            var getResponse =
                await adminClient.GetAsync(
                    $"/api/rentals/{createdRental.Id}");

            Assert.Equal(
                HttpStatusCode.NotFound,
                getResponse.StatusCode);
        }
        finally
        {
            await adminAuth.DeleteCurrentUserAsync();
            await customerAuth.DeleteCurrentUserAsync();
        }
    }

    [Fact]
    public async Task DeleteRental_WhenRentalDoesNotExistAndUserIsAdmin_ReturnsNotFound()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var admin =
            TestUserData.Admin();

        try
        {
            var loginResponse =
                await auth.RegisterAndLoginAsync(admin);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            // Act
            var response =
                await client.DeleteAsync(
                    $"/api/rentals/{int.MaxValue}");

            // Assert
            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode);
        }
        finally
        {
            await auth.DeleteCurrentUserAsync();
        }
    }

    [Fact]
    public async Task DeleteRental_WhenUserIsWorker_ReturnsForbidden()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var worker =
            TestUserData.Worker();

        try
        {
            var loginResponse =
                await auth.RegisterAndLoginAsync(worker);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            // Act
            var response =
                await client.DeleteAsync(
                    $"/api/rentals/{int.MaxValue}");

            // Assert
            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode);
        }
        finally
        {
            await auth.DeleteCurrentUserAsync();
        }
    }

    [Fact]
    public async Task DeleteRental_WhenUserIsCustomer_ReturnsForbidden()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var customer =
            TestUserData.Customer();

        try
        {
            var loginResponse =
                await auth.RegisterAndLoginAsync(customer);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            // Act
            var response =
                await client.DeleteAsync(
                    $"/api/rentals/{int.MaxValue}");

            // Assert
            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode);
        }
        finally
        {
            await auth.DeleteCurrentUserAsync();
        }
    }

    [Fact]
    public async Task DeleteRental_WhenUserIsAnonymous_ReturnsUnauthorized()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        // Act
        var response =
            await client.DeleteAsync(
                $"/api/rentals/{int.MaxValue}");

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }
}
