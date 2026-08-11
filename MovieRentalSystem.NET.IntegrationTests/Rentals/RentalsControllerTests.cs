using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using MovieRentalSystem.NET.IntegrationTests.Fixtures;
using MovieRentalSystem.NET.IntegrationTests.Helpers;

namespace MovieRentalSystem.NET.IntegrationTests.Rentals;

[Collection("Aspire collection")]
public class RentalsControllerTests
{
    private readonly AspireFixture _fixture;
    public RentalsControllerTests(
    AspireFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Admin_CanGetRentals()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var login =
            await auth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            login.IsSuccessStatusCode);

        // Act
        var response =
            await client.GetAsync(
                "/api/rentals");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Customer_CannotGetRentals()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var user =
            TestUserData.Customer();

        var login =
            await auth.RegisterAndLoginAsync(user);

        Assert.True(
            login.IsSuccessStatusCode);

        // Act
        var response =
            await client.GetAsync(
                "/api/rentals");

        // Assert
        Assert.Equal(
            HttpStatusCode.Forbidden,
            response.StatusCode);
    }

    [Fact]
    public async Task AnonymousUser_CannotGetRentals()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        // Act
        var response =
            await client.GetAsync(
                "/api/rentals");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_CanGetRentalById()
    {
        // Arrange
        var customerClient =
            _fixture.CreateClient();

        var customerAuth =
            _fixture.CreateAuthHelper(customerClient);

        var customer =
            TestUserData.Customer();

        var customerLogin =
            await customerAuth.RegisterAndLoginAsync(customer);

        Assert.True(
            customerLogin.IsSuccessStatusCode);

        var currentUser =
            await customerAuth.GetCurrentUserAsync();

        Assert.NotNull(currentUser);

        // Create rental
        var rentalRequest = new
        {
            UserId = currentUser!.Id,
            MoviePhysicalCopyId = 1,
            RentalStartDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        var createResponse =
            await customerClient.PostAsJsonAsync(
                "/api/rentals",
                rentalRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            createResponse.StatusCode);

        var rental =
            await createResponse.Content
                .ReadFromJsonAsync<JsonElement>();

        Assert.True(
            rental.TryGetProperty("id", out var idProperty));

        var rentalId =
            idProperty.GetInt32();

        var adminClient =
            _fixture.CreateClient();

        var adminAuth =
            _fixture.CreateAuthHelper(adminClient);

        var adminLogin =
            await adminAuth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            adminLogin.IsSuccessStatusCode);

        // Act
        var response =
            await adminClient.GetAsync(
                $"/api/rentals/{rentalId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_GetRental_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var login =
            await auth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            login.IsSuccessStatusCode);

        // Act
        var response =
            await client.GetAsync(
                "/api/rentals/999999");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Customer_CanCreateRental()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var user =
            TestUserData.Customer();

        var login =
            await auth.RegisterAndLoginAsync(user);

        Assert.True(
            login.IsSuccessStatusCode);

        var currentUser =
            await auth.GetCurrentUserAsync();

        Assert.NotNull(currentUser);

        var request = new
        {
            UserId = currentUser!.Id,
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
            HttpStatusCode.Created,
            response.StatusCode);
    }

    [Fact]
    public async Task AnonymousUser_CannotCreateRental()
    {
        // Arrange
        var client =
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
            HttpStatusCode.InternalServerError,
            response.StatusCode);
    }

    [Fact]
    public async Task Customer_CreateRental_WithInvalidCopy_ReturnsInternalServerError()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var user =
            TestUserData.Customer();

        var login =
            await auth.RegisterAndLoginAsync(user);

        Assert.True(
            login.IsSuccessStatusCode);

        var currentUser =
            await auth.GetCurrentUserAsync();

        Assert.NotNull(currentUser);

        var request = new
        {
            UserId = currentUser!.Id,
            MoviePhysicalCopyId = 999999,
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
            HttpStatusCode.InternalServerError,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_CanUpdateRental()
    {
        // Arrange
        var customerClient =
            _fixture.CreateClient();

        var customerAuth =
            _fixture.CreateAuthHelper(customerClient);

        var customer =
            TestUserData.Customer();

        var customerLogin =
            await customerAuth.RegisterAndLoginAsync(customer);

        Assert.True(
            customerLogin.IsSuccessStatusCode);

        var currentUser =
            await customerAuth.GetCurrentUserAsync();

        Assert.NotNull(currentUser);

        // Create rental
        var createRentalRequest = new
        {
            UserId = currentUser!.Id,
            MoviePhysicalCopyId = 1,
            RentalStartDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        var createResponse =
            await customerClient.PostAsJsonAsync(
                "/api/rentals",
                createRentalRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            createResponse.StatusCode);

        var rental =
            await createResponse.Content
                .ReadFromJsonAsync<JsonElement>();

        Assert.True(
            rental.TryGetProperty("id", out var idProperty));

        var rentalId =
            idProperty.GetInt32();

        // Login as admin using a separate client
        var adminClient =
            _fixture.CreateClient();

        var adminAuth =
            _fixture.CreateAuthHelper(adminClient);

        var adminLogin =
            await adminAuth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            adminLogin.IsSuccessStatusCode);

        var updateRequest = new
        {
            RentalStartDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(10),
            ReturnDate = (DateTime?) null,
            TotalPrice = 50m,
            Status = 1
        };

        // Act
        var response =
            await adminClient.PutAsJsonAsync(
                $"/api/rentals/{rentalId}",
                updateRequest);

        // Assert
        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);
    }

    [Fact]
    public async Task Customer_CannotUpdateRental()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var user =
            TestUserData.Customer();

        var login =
            await auth.RegisterAndLoginAsync(user);

        Assert.True(
            login.IsSuccessStatusCode);

        var request = new
        {
            RentalStartDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(10),
            ReturnDate = (DateTime?) null,
            TotalPrice = 50m,
            Status = 1
        };

        // Act
        var response =
            await client.PutAsJsonAsync(
                "/api/rentals/1",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Forbidden,
            response.StatusCode);
    }


    [Fact]
    public async Task Admin_CanDeleteRental()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        // Create customer
        var customer =
            TestUserData.Customer();

        var customerAuth =
            _fixture.CreateAuthHelper(client);

        var customerLogin =
            await customerAuth.RegisterAndLoginAsync(customer);

        Assert.True(
            customerLogin.IsSuccessStatusCode);

        var currentUser =
            await customerAuth.GetCurrentUserAsync();

        Assert.NotNull(currentUser);

        // Create rental
        var rentalRequest = new
        {
            UserId = currentUser!.Id,
            MoviePhysicalCopyId = 1,
            RentalStartDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        var createRentalResponse =
            await client.PostAsJsonAsync(
                "/api/rentals",
                rentalRequest);

        Assert.Equal(
            HttpStatusCode.Created,
            createRentalResponse.StatusCode);

        var rental =
            await createRentalResponse.Content
                .ReadFromJsonAsync<JsonElement>();

        Assert.True(
            rental.TryGetProperty("id", out var idProperty));

        var rentalId =
            idProperty.GetInt32();

        // Login as admin
        var adminAuth =
            _fixture.CreateAuthHelper(client);

        var adminLogin =
            await adminAuth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            adminLogin.IsSuccessStatusCode);

        // Act
        var response =
            await client.DeleteAsync(
                $"/api/rentals/{rentalId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);
    }


    [Fact]
    public async Task Worker_CannotDeleteRental()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var login =
            await auth.LoginUserAsync(
                "worker@test.com",
                "Worker123!");

        Assert.True(
            login.IsSuccessStatusCode);

        // Act
        var response =
            await client.DeleteAsync(
                "/api/rentals/1");

        // Assert
        Assert.Equal(
            HttpStatusCode.Forbidden,
            response.StatusCode);
    }
}
