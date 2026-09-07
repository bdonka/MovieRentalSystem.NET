using System.Net;
using System.Net.Http.Json;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Dtos;
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
    public async Task GetUsers_WhenUserIsAnonymous_ReturnsOkWithUsers()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        // Act
        var response =
            await client.GetAsync(
                "/api/users");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var result =
            await response.Content
                .ReadFromJsonAsync<PagedResponse<UserDto>>();

        Assert.NotNull(result);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task GetUsers_WhenUserIsCustomer_ReturnsForbidden()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var customer =
            TestUserData.Customer();

        UserDto? createdUser = null;

        try
        {
            var loginResponse =
                await auth.RegisterAndLoginAsync(customer);

            Assert.True(
                loginResponse.IsSuccessStatusCode);

            createdUser =
                await auth.GetCurrentUserAsync();

            Assert.NotNull(createdUser);

            // Act
            var response =
                await client.GetAsync(
                    "/api/users");

            // Assert
            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode);
        }
        finally
        {
            if (createdUser is not null)
            {
                await client.DeleteAsync(
                    $"/api/users/{createdUser.Id}");
            }
        }
    }

    [Fact]
    public async Task GetUsers_WhenUserIsAdmin_ReturnsOkWithUsers()
    {
        // Arrange
        using var adminClient =
            _fixture.CreateClient();

        var adminAuth =
            _fixture.CreateAuthHelper(adminClient);

        var admin =
            TestUserData.Admin();

        UserDto? createdAdmin = null;

        using var customerClient =
            _fixture.CreateClient();

        var customerAuth =
            _fixture.CreateAuthHelper(customerClient);

        var customer =
            TestUserData.Customer();

        UserDto? createdCustomer = null;

        try
        {
            var adminLoginResponse =
                await adminAuth.RegisterAndLoginAsync(admin);

            Assert.True(
                adminLoginResponse.IsSuccessStatusCode);

            createdAdmin =
                await adminAuth.GetCurrentUserAsync();

            Assert.NotNull(createdAdmin);

            var customerLoginResponse =
                await customerAuth.RegisterAndLoginAsync(customer);

            Assert.True(
                customerLoginResponse.IsSuccessStatusCode);

            createdCustomer =
                await customerAuth.GetCurrentUserAsync();

            Assert.NotNull(createdCustomer);

            // Act
            var response =
                await adminClient.GetAsync(
                    "/api/users");

            // Assert
            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            var result =
                await response.Content
                    .ReadFromJsonAsync<PagedResponse<UserDto>>();

            Assert.NotNull(result);
            Assert.NotNull(result.Data);

            Assert.Contains(
                result.Data,
                user => user.Id == createdCustomer.Id);
        }
        finally
        {
            if (createdCustomer is not null)
            {
                await customerClient.DeleteAsync(
                    $"/api/users/{createdCustomer.Id}");
            }

            if (createdAdmin is not null)
            {
                await adminClient.DeleteAsync(
                    $"/api/users/{createdAdmin.Id}");
            }
        }
    }

    [Fact]
    public async Task GetUserById_WhenUserExistsAndRequesterIsAdmin_ReturnsOkWithUser()
    {
        // Arrange
        using var customerClient =
            _fixture.CreateClient();

        var customerAuth =
            _fixture.CreateAuthHelper(customerClient);

        var customer =
            TestUserData.Customer();

        UserDto? createdCustomer = null;

        var customerLoginResponse =
            await customerAuth.RegisterAndLoginAsync(customer);

        Assert.True(
            customerLoginResponse.IsSuccessStatusCode);

        createdCustomer =
            await customerAuth.GetCurrentUserAsync();

        Assert.NotNull(createdCustomer);

        using var adminClient =
            _fixture.CreateClient();

        var adminAuth =
            _fixture.CreateAuthHelper(adminClient);

        var admin =
            TestUserData.Admin();

        UserDto? createdAdmin = null;

        try
        {
            var adminLoginResponse =
                await adminAuth.RegisterAndLoginAsync(admin);

            Assert.True(
                adminLoginResponse.IsSuccessStatusCode);

            createdAdmin =
                await adminAuth.GetCurrentUserAsync();

            Assert.NotNull(createdAdmin);

            // Act
            var response =
                await adminClient.GetAsync(
                    $"/api/users/{createdCustomer.Id}");

            // Assert
            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);

            var returnedUser =
                await response.Content
                    .ReadFromJsonAsync<UserDto>();

            Assert.NotNull(returnedUser);

            Assert.Equal(
                createdCustomer.Id,
                returnedUser.Id);

            Assert.Equal(
                createdCustomer.UserName,
                returnedUser.UserName);

            Assert.Equal(
                createdCustomer.Email,
                returnedUser.Email);
        }
        finally
        {
            if (createdCustomer is not null)
            {
                await customerClient.DeleteAsync(
                    $"/api/users/{createdCustomer.Id}");
            }

            if (createdAdmin is not null)
            {
                await adminClient.DeleteAsync(
                    $"/api/users/{createdAdmin.Id}");
            }
        }
    }

    [Fact]
    public async Task GetUserById_WhenUserDoesNotExistAndRequesterIsAdmin_ReturnsNotFound()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var admin =
            TestUserData.Admin();

        UserDto? createdAdmin = null;

        var loginResponse =
            await auth.RegisterAndLoginAsync(admin);

        Assert.True(
            loginResponse.IsSuccessStatusCode);

        createdAdmin =
            await auth.GetCurrentUserAsync();

        Assert.NotNull(createdAdmin);

        var nonExistingId =
            Guid.NewGuid().ToString();

        try
        {
            // Act
            var response =
                await client.GetAsync(
                    $"/api/users/{nonExistingId}");

            // Assert
            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode);
        }
        finally
        {
            if (createdAdmin is not null)
            {
                await client.DeleteAsync(
                    $"/api/users/{createdAdmin.Id}");
            }
        }
    }

    [Fact]
    public async Task CreateUser_WhenRequesterIsAdmin_ReturnsCreatedUser()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var admin =
            TestUserData.Admin();

        UserDto? createdAdmin = null;
        UserDto? createdUser = null;

        var loginResponse =
            await auth.RegisterAndLoginAsync(admin);

        Assert.True(
            loginResponse.IsSuccessStatusCode);

        createdAdmin =
            await auth.GetCurrentUserAsync();

        Assert.NotNull(createdAdmin);

        var user =
            TestUserData.Customer();

        var request = new
        {
            user.UserName,
            user.Email,
            user.Password
        };

        try
        {
            // Act
            var response =
                await client.PostAsJsonAsync(
                    "/api/users",
                    request);

            // Assert
            Assert.Equal(
                HttpStatusCode.Created,
                response.StatusCode);

            createdUser =
                await response.Content
                    .ReadFromJsonAsync<UserDto>();

            Assert.NotNull(createdUser);

            Assert.False(
                string.IsNullOrWhiteSpace(
                    createdUser.Id));

            Assert.Equal(
                request.UserName,
                createdUser.UserName);

            Assert.Equal(
                request.Email,
                createdUser.Email);
        }
        finally
        {
            if (createdUser is not null)
            {
                await client.DeleteAsync(
                    $"/api/users/{createdUser.Id}");
            }

            if (createdAdmin is not null)
            {
                await client.DeleteAsync(
                    $"/api/users/{createdAdmin.Id}");
            }
        }
    }

    [Fact]
    public async Task CreateUser_WhenRequesterIsCustomer_ReturnsForbidden()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var customer =
            TestUserData.Customer();

        UserDto? createdCustomer = null;

        var loginResponse =
            await auth.RegisterAndLoginAsync(customer);

        Assert.True(
            loginResponse.IsSuccessStatusCode);

        createdCustomer =
            await auth.GetCurrentUserAsync();

        Assert.NotNull(createdCustomer);

        var userToCreate =
            TestUserData.Customer();

        var request = new
        {
            userToCreate.UserName,
            userToCreate.Email,
            userToCreate.Password
        };

        try
        {
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
        finally
        {
            if (createdCustomer is not null)
            {
                await client.DeleteAsync(
                    $"/api/users/{createdCustomer.Id}");
            }
        }
    }

    [Fact]
    public async Task CreateUser_WhenRequesterIsAnonymous_ReturnsUnauthorized()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var request = new
        {
            UserName = $"anonymous_{Guid.NewGuid():N}",
            Email = $"anonymous_{Guid.NewGuid():N}@test.com",
            Password = "Password123!"
        };

        // Act
        var response =
            await client.PostAsJsonAsync(
                "/api/users",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task UpdateUser_WhenUserExistsAndRequesterIsAdmin_ReturnsNoContentAndUpdatesUser()
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

        var createdCustomer =
            await customerAuth.GetCurrentUserAsync();

        Assert.NotNull(createdCustomer);

        using var adminClient =
            _fixture.CreateClient();

        var adminAuth =
            _fixture.CreateAuthHelper(adminClient);

        var admin =
            TestUserData.Admin();

        UserDto? createdAdmin = null;

        var adminLoginResponse =
            await adminAuth.RegisterAndLoginAsync(admin);

        Assert.True(
            adminLoginResponse.IsSuccessStatusCode);

        createdAdmin =
            await adminAuth.GetCurrentUserAsync();

        Assert.NotNull(createdAdmin);

        var updatedUserName =
            $"updated_{Guid.NewGuid():N}";

        var updatedEmail =
            $"{updatedUserName}@test.com";

        var request = new
        {
            UserName = updatedUserName,
            Email = updatedEmail
        };

        try
        {
            // Act
            var response =
                await adminClient.PutAsJsonAsync(
                    $"/api/users/{createdCustomer.Id}",
                    request);

            // Assert
            Assert.Equal(
                HttpStatusCode.NoContent,
                response.StatusCode);

            var getResponse =
                await adminClient.GetAsync(
                    $"/api/users/{createdCustomer.Id}");

            Assert.Equal(
                HttpStatusCode.OK,
                getResponse.StatusCode);

            var updatedUser =
                await getResponse.Content
                    .ReadFromJsonAsync<UserDto>();

            Assert.NotNull(updatedUser);

            Assert.Equal(
                createdCustomer.Id,
                updatedUser.Id);

            Assert.Equal(
                updatedUserName,
                updatedUser.UserName);

            Assert.Equal(
                updatedEmail,
                updatedUser.Email);
        }
        finally
        {
            await customerClient.DeleteAsync(
                $"/api/users/{createdCustomer.Id}");

            if (createdAdmin is not null)
            {
                await adminClient.DeleteAsync(
                    $"/api/users/{createdAdmin.Id}");
            }
        }
    }

    [Fact]
    public async Task UpdateUser_WhenUserDoesNotExistAndRequesterIsAdmin_ReturnsNotFound()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var admin =
            TestUserData.Admin();

        UserDto? createdAdmin = null;

        var loginResponse =
            await auth.RegisterAndLoginAsync(admin);

        Assert.True(
            loginResponse.IsSuccessStatusCode);

        createdAdmin =
            await auth.GetCurrentUserAsync();

        Assert.NotNull(createdAdmin);

        var nonExistingId =
            Guid.NewGuid().ToString();

        var request = new
        {
            UserName = $"updated_{Guid.NewGuid():N}",
            Email = $"updated_{Guid.NewGuid():N}@test.com"
        };

        try
        {
            // Act
            var response =
                await client.PutAsJsonAsync(
                    $"/api/users/{nonExistingId}",
                    request);

            // Assert
            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode);
        }
        finally
        {
            if (createdAdmin is not null)
            {
                await client.DeleteAsync(
                    $"/api/users/{createdAdmin.Id}");
            }
        }
    }

    [Fact]
    public async Task UpdateUser_WhenRequesterIsCustomer_ReturnsForbidden()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var customer =
            TestUserData.Customer();

        UserDto? createdCustomer = null;

        var loginResponse =
            await auth.RegisterAndLoginAsync(customer);

        Assert.True(
            loginResponse.IsSuccessStatusCode);

        createdCustomer =
            await auth.GetCurrentUserAsync();

        Assert.NotNull(createdCustomer);

        var request = new
        {
            UserName = $"updated_{Guid.NewGuid():N}",
            Email = $"updated_{Guid.NewGuid():N}@test.com"
        };

        var nonExistingId =
            Guid.NewGuid().ToString();

        try
        {
            // Act
            var response =
                await client.PutAsJsonAsync(
                    $"/api/users/{nonExistingId}",
                    request);

            // Assert
            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode);
        }
        finally
        {
            if (createdCustomer is not null)
            {
                await client.DeleteAsync(
                    $"/api/users/{createdCustomer.Id}");
            }
        }
    }

    [Fact]
    public async Task UpdateUser_WhenRequesterIsAnonymous_ReturnsUnauthorized()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var request = new
        {
            UserName = $"updated_{Guid.NewGuid():N}",
            Email = $"updated_{Guid.NewGuid():N}@test.com"
        };

        var nonExistingId =
            Guid.NewGuid().ToString();

        // Act
        var response =
            await client.PutAsJsonAsync(
                $"/api/users/{nonExistingId}",
                request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task DeleteUser_WhenUserExistsAndRequesterIsAdmin_ReturnsNoContentAndUserIsNotFound()
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

        var createdCustomer =
            await customerAuth.GetCurrentUserAsync();

        Assert.NotNull(createdCustomer);

        using var adminClient =
            _fixture.CreateClient();

        var adminAuth =
            _fixture.CreateAuthHelper(adminClient);

        var admin =
            TestUserData.Admin();

        UserDto? createdAdmin = null;

        var adminLoginResponse =
            await adminAuth.RegisterAndLoginAsync(admin);

        Assert.True(
            adminLoginResponse.IsSuccessStatusCode);

        createdAdmin =
            await adminAuth.GetCurrentUserAsync();

        Assert.NotNull(createdAdmin);

        try
        {
            // Act
            var response =
                await adminClient.DeleteAsync(
                    $"/api/users/{createdCustomer.Id}");

            // Assert
            Assert.Equal(
                HttpStatusCode.NoContent,
                response.StatusCode);

            var getResponse =
                await adminClient.GetAsync(
                    $"/api/users/{createdCustomer.Id}");

            Assert.Equal(
                HttpStatusCode.NotFound,
                getResponse.StatusCode);
        }
        finally
        {
            if (createdAdmin is not null)
            {
                await adminClient.DeleteAsync(
                    $"/api/users/{createdAdmin.Id}");
            }
        }
    }

    [Fact]
    public async Task DeleteUser_WhenUserDoesNotExistAndRequesterIsAdmin_ReturnsNotFound()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var admin =
            TestUserData.Admin();

        UserDto? createdAdmin = null;

        var loginResponse =
            await auth.RegisterAndLoginAsync(admin);

        Assert.True(
            loginResponse.IsSuccessStatusCode);

        createdAdmin =
            await auth.GetCurrentUserAsync();

        Assert.NotNull(createdAdmin);

        var nonExistingId =
            Guid.NewGuid().ToString();

        try
        {
            // Act
            var response =
                await client.DeleteAsync(
                    $"/api/users/{nonExistingId}");

            // Assert
            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode);
        }
        finally
        {
            if (createdAdmin is not null)
            {
                await client.DeleteAsync(
                    $"/api/users/{createdAdmin.Id}");
            }
        }
    }

    [Fact]
    public async Task DeleteUser_WhenRequesterIsCustomer_ReturnsForbidden()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var customer =
            TestUserData.Customer();

        UserDto? createdCustomer = null;

        var loginResponse =
            await auth.RegisterAndLoginAsync(customer);

        Assert.True(
            loginResponse.IsSuccessStatusCode);

        createdCustomer =
            await auth.GetCurrentUserAsync();

        Assert.NotNull(createdCustomer);

        try
        {
            // Act
            var response =
                await client.DeleteAsync(
                    $"/api/users/{createdCustomer.Id}");

            // Assert
            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode);
        }
        finally
        {
            if (createdCustomer is not null)
            {
                await client.DeleteAsync(
                    $"/api/users/{createdCustomer.Id}");
            }
        }
    }

    [Fact]
    public async Task DeleteUser_WhenRequesterIsAnonymous_ReturnsUnauthorized()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var nonExistingId =
            Guid.NewGuid().ToString();

        // Act
        var response =
            await client.DeleteAsync(
                $"/api/users/{nonExistingId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task AssignRole_WhenRequesterIsAdmin_ReturnsOk()
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

        var createdCustomer =
            await customerAuth.GetCurrentUserAsync();

        Assert.NotNull(createdCustomer);

        using var adminClient =
            _fixture.CreateClient();

        var adminAuth =
            _fixture.CreateAuthHelper(adminClient);

        var admin =
            TestUserData.Admin();

        UserDto? createdAdmin = null;

        var adminLoginResponse =
            await adminAuth.RegisterAndLoginAsync(admin);

        Assert.True(
            adminLoginResponse.IsSuccessStatusCode);

        createdAdmin =
            await adminAuth.GetCurrentUserAsync();

        Assert.NotNull(createdAdmin);

        try
        {
            // Act
            var response =
                await adminClient.PostAsync(
                    $"/api/users/{createdCustomer.Id}/roles/Admin",
                    null);

            // Assert
            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);
        }
        finally
        {
            await customerClient.DeleteAsync(
                $"/api/users/{createdCustomer.Id}");

            if (createdAdmin is not null)
            {
                await adminClient.DeleteAsync(
                    $"/api/users/{createdAdmin.Id}");
            }
        }
    }

    [Fact]
    public async Task AssignRole_WhenRequesterIsCustomer_ReturnsForbidden()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var customer =
            TestUserData.Customer();

        UserDto? createdCustomer = null;

        var loginResponse =
            await auth.RegisterAndLoginAsync(customer);

        Assert.True(
            loginResponse.IsSuccessStatusCode);

        createdCustomer =
            await auth.GetCurrentUserAsync();

        Assert.NotNull(createdCustomer);

        var targetUserId =
            Guid.NewGuid().ToString();

        try
        {
            // Act
            var response =
                await client.PostAsync(
                    $"/api/users/{targetUserId}/roles/Admin",
                    null);

            // Assert
            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode);
        }
        finally
        {
            if (createdCustomer is not null)
            {
                await client.DeleteAsync(
                    $"/api/users/{createdCustomer.Id}");
            }
        }
    }

    [Fact]
    public async Task AssignRole_WhenRequesterIsAnonymous_ReturnsUnauthorized()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var targetUserId =
            Guid.NewGuid().ToString();

        // Act
        var response =
            await client.PostAsync(
                $"/api/users/{targetUserId}/roles/Admin",
                null);

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task RemoveRole_WhenUserHasRoleAndRequesterIsAdmin_ReturnsOk()
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

        var createdCustomer =
            await customerAuth.GetCurrentUserAsync();

        Assert.NotNull(createdCustomer);

        using var adminClient =
            _fixture.CreateClient();

        var adminAuth =
            _fixture.CreateAuthHelper(adminClient);

        var admin =
            TestUserData.Admin();

        UserDto? createdAdmin = null;

        var adminLoginResponse =
            await adminAuth.RegisterAndLoginAsync(admin);

        Assert.True(
            adminLoginResponse.IsSuccessStatusCode);

        createdAdmin =
            await adminAuth.GetCurrentUserAsync();

        Assert.NotNull(createdAdmin);

        var assignResponse =
            await adminClient.PostAsync(
                $"/api/users/{createdCustomer.Id}/roles/Admin",
                null);

        Assert.Equal(
            HttpStatusCode.OK,
            assignResponse.StatusCode);

        try
        {
            // Act
            var removeResponse =
                await adminClient.DeleteAsync(
                    $"/api/users/{createdCustomer.Id}/roles/Admin");

            // Assert
            Assert.Equal(
                HttpStatusCode.OK,
                removeResponse.StatusCode);
        }
        finally
        {
            await customerClient.DeleteAsync(
                $"/api/users/{createdCustomer.Id}");

            if (createdAdmin is not null)
            {
                await adminClient.DeleteAsync(
                    $"/api/users/{createdAdmin.Id}");
            }
        }
    }

    [Fact]
    public async Task RemoveRole_WhenRequesterIsCustomer_ReturnsForbidden()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var customer =
            TestUserData.Customer();

        UserDto? createdCustomer = null;

        var loginResponse =
            await auth.RegisterAndLoginAsync(customer);

        Assert.True(
            loginResponse.IsSuccessStatusCode);

        createdCustomer =
            await auth.GetCurrentUserAsync();

        Assert.NotNull(createdCustomer);

        var targetUserId =
            Guid.NewGuid().ToString();

        try
        {
            // Act
            var response =
                await client.DeleteAsync(
                    $"/api/users/{targetUserId}/roles/Admin");

            // Assert
            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode);
        }
        finally
        {
            if (createdCustomer is not null)
            {
                await client.DeleteAsync(
                    $"/api/users/{createdCustomer.Id}");
            }
        }
    }

    [Fact]
    public async Task RemoveRole_WhenRequesterIsAnonymous_ReturnsUnauthorized()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var targetUserId =
            Guid.NewGuid().ToString();

        // Act
        var response =
            await client.DeleteAsync(
                $"/api/users/{targetUserId}/roles/Admin");

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }
}
