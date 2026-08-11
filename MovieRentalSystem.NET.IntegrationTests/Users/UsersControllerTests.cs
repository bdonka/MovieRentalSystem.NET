using System.Net;
using System.Net.Http.Json;
using MovieRentalSystem.NET.IntegrationTests.Fixtures;
using MovieRentalSystem.NET.IntegrationTests.Helpers;

namespace MovieRentalSystem.NET.IntegrationTests.Users;

[Collection("Aspire collection")]
public class UsersControllerTests
{
    private readonly AspireFixture _fixture;

    public UsersControllerTests(AspireFixture fixture)
    {
        _fixture = fixture;
    }


    [Fact]
    public async Task AnonymousUser_CannotGetUsers()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        // Act
        var response =
            await client.GetAsync(
                "/api/users");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Customer_CannotGetUsers()
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
                "/api/users");

        // Assert
        Assert.Equal(
            HttpStatusCode.Forbidden,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_CanGetUsers()
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
                "/api/users");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_CanGetUserById()
    {
        // Arrange
        var customerClient =
            _fixture.CreateClient();

        var customerAuth =
            _fixture.CreateAuthHelper(
                customerClient);

        var user =
            TestUserData.Customer();

        var register =
            await customerAuth.RegisterAndLoginAsync(
                user);

        Assert.True(
            register.IsSuccessStatusCode);

        var createdUser =
            await customerAuth.GetCurrentUserAsync();

        Assert.NotNull(
            createdUser);

        // Tworzymy osobnego klienta dla administratora.
        var adminClient =
            _fixture.CreateClient();

        var adminAuth =
            _fixture.CreateAuthHelper(
                adminClient);

        var login =
            await adminAuth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            login.IsSuccessStatusCode);

        // Act
        var response =
            await adminClient.GetAsync(
                $"/api/users/{createdUser!.Id}");

        // Assert
        var responseBody =
            await response.Content.ReadAsStringAsync();

        Assert.True(
            response.StatusCode == HttpStatusCode.OK,
            $"Expected 200 OK, but received {(int) response.StatusCode} {response.StatusCode}. Response: {responseBody}");
    }

    [Fact]
    public async Task GetUser_WithInvalidId_ReturnsNotFound()
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
                "/api/users/non-existing-id");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_CanCreateUser()
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

        var id =
            Guid.NewGuid();

        var request = new
        {
            UserName = $"created_user_{id}",
            Email = $"created_{id}@test.com",
            Password = "Password123!"
        };

        // Act
        var response =
            await client.PostAsJsonAsync(
                "/api/users",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);
    }

    [Fact]
    public async Task Customer_CannotCreateUser()
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

        var id =
            Guid.NewGuid();

        var request = new
        {
            UserName = $"new_user_{id}",
            Email = $"new_{id}@test.com",
            Password = "Password123!"
        };

        // Act
        var response =
            await client.PostAsJsonAsync(
                "/api/users",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Forbidden,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_CanUpdateUser()
    {
        // Arrange
        var customerClient =
            _fixture.CreateClient();

        var customerAuth =
            _fixture.CreateAuthHelper(
                customerClient);

        var user =
            TestUserData.Customer();

        var register =
            await customerAuth.RegisterAndLoginAsync(
                user);

        Assert.True(
            register.IsSuccessStatusCode);

        var createdUser =
            await customerAuth.GetCurrentUserAsync();

        Assert.NotNull(
            createdUser);

        var adminClient =
            _fixture.CreateClient();

        var adminAuth =
            _fixture.CreateAuthHelper(
                adminClient);

        var login =
            await adminAuth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            login.IsSuccessStatusCode);

        var id =
            Guid.NewGuid();

        var request = new
        {
            UserName = $"updated_customer_{id}",
            Email = $"updated_{id}@test.com"
        };

        // Act
        var response =
            await adminClient.PutAsJsonAsync(
                $"/api/users/{createdUser!.Id}",
                request);

        // Assert
        var responseBody =
            await response.Content.ReadAsStringAsync();

        Assert.True(
            response.StatusCode == HttpStatusCode.NoContent,
            $"Expected 204 No Content, but received {(int) response.StatusCode} {response.StatusCode}. Response: {responseBody}");
    }

    [Fact]
    public async Task Admin_CanDeleteUser()
    {
        // Arrange
        var customerClient =
            _fixture.CreateClient();

        var customerAuth =
            _fixture.CreateAuthHelper(
                customerClient);

        var user =
            TestUserData.Customer();

        var customerLogin =
            await customerAuth.RegisterAndLoginAsync(
                user);

        Assert.True(
            customerLogin.IsSuccessStatusCode);

        var createdUser =
            await customerAuth.GetCurrentUserAsync();

        Assert.NotNull(
            createdUser);

        var adminClient =
            _fixture.CreateClient();

        var adminAuth =
            _fixture.CreateAuthHelper(
                adminClient);

        var adminLogin =
            await adminAuth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            adminLogin.IsSuccessStatusCode);

        // Act
        var response =
            await adminClient.DeleteAsync(
                $"/api/users/{createdUser!.Id}");

        // Assert
        var responseBody =
            await response.Content.ReadAsStringAsync();

        Assert.True(
            response.StatusCode == HttpStatusCode.NoContent,
            $"Expected 204 No Content, but received {(int) response.StatusCode} {response.StatusCode}. Response: {responseBody}");
    }

    [Fact]
    public async Task Customer_CannotDeleteUser()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var user =
            TestUserData.Customer();

        var register =
            await auth.RegisterAndLoginAsync(user);

        Assert.True(
            register.IsSuccessStatusCode);

        // Act
        var response =
            await client.DeleteAsync(
                $"/api/users/{user.Id}");

        // Assert
        Assert.Equal(
            HttpStatusCode.Forbidden,
            response.StatusCode);
    }

    [Fact]
    public async Task Admin_CanAssignRole()
    {
        // Arrange
        var customerClient =
            _fixture.CreateClient();

        var customerAuth =
            _fixture.CreateAuthHelper(
                customerClient);

        var user =
            TestUserData.Customer();

        var register =
            await customerAuth.RegisterAndLoginAsync(
                user);

        Assert.True(
            register.IsSuccessStatusCode);

        var createdUser =
            await customerAuth.GetCurrentUserAsync();

        Assert.NotNull(createdUser);

        var adminClient =
            _fixture.CreateClient();

        var adminAuth =
            _fixture.CreateAuthHelper(
                adminClient);

        var login =
            await adminAuth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            login.IsSuccessStatusCode);

        // Act
        var response =
            await adminClient.PostAsync(
                $"/api/users/{createdUser!.Id}/roles/Admin",
                null);

        // Assert
        var responseBody =
            await response.Content.ReadAsStringAsync();

        Assert.True(
            response.StatusCode == HttpStatusCode.OK,
            $"Expected 200 OK, but received {(int) response.StatusCode} {response.StatusCode}. Response: {responseBody}");
    }

    [Fact]
    public async Task Admin_CanRemoveRole()
    {
        // Arrange
        var customerClient =
            _fixture.CreateClient();

        var customerAuth =
            _fixture.CreateAuthHelper(
                customerClient);

        var user =
            TestUserData.Customer();

        var register =
            await customerAuth.RegisterAndLoginAsync(
                user);

        Assert.True(
            register.IsSuccessStatusCode);

        var createdUser =
            await customerAuth.GetCurrentUserAsync();

        Assert.NotNull(
            createdUser);

        var adminClient =
            _fixture.CreateClient();

        var adminAuth =
            _fixture.CreateAuthHelper(
                adminClient);

        var login =
            await adminAuth.LoginUserAsync(
                "admin@test.com",
                "Admin123!");

        Assert.True(
            login.IsSuccessStatusCode);

        var assignResponse =
            await adminClient.PostAsync(
                $"/api/users/{createdUser!.Id}/roles/Admin",
                null);

        var assignResponseBody =
            await assignResponse.Content.ReadAsStringAsync();

        Assert.True(
            assignResponse.StatusCode == HttpStatusCode.OK,
            $"Expected 200 OK when assigning role, but received {(int) assignResponse.StatusCode} {assignResponse.StatusCode}. Response: {assignResponseBody}");

        // Act
        var removeResponse =
            await adminClient.DeleteAsync(
                $"/api/users/{createdUser.Id}/roles/Admin");

        // Assert
        var removeResponseBody =
            await removeResponse.Content.ReadAsStringAsync();

        Assert.True(
            removeResponse.StatusCode == HttpStatusCode.OK,
            $"Expected 200 OK when removing role, but received {(int) removeResponse.StatusCode} {removeResponse.StatusCode}. Response: {removeResponseBody}");
    }

    [Fact]
    public async Task Customer_CannotAssignRole()
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
            await client.PostAsync(
                $"/api/users/{user.Id}/roles/Admin",
                null);

        // Assert
        Assert.Equal(
            HttpStatusCode.Forbidden,
            response.StatusCode);
    }
}
