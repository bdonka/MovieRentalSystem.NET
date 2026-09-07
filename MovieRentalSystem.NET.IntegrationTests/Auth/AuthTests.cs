using System.Net;
using System.Net.Http.Json;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.IntegrationTests.Fixtures;
using MovieRentalSystem.NET.IntegrationTests.Helpers;

namespace MovieRentalSystem.NET.IntegrationTests.Auth;

[Collection("Aspire collection")]
public class AuthTests
{
    private readonly AspireFixture _fixture;

    public AuthTests(AspireFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Register_WhenDataIsValid_ReturnsSuccess()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var user =
            TestUserData.Customer();

        // Act
        var response =
            await auth.RegisterUserAsync(user);

        // Assert
        Assert.True(
            response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Register_WhenEmailAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var user =
            TestUserData.Customer();

        var firstResponse =
            await auth.RegisterUserAsync(user);

        Assert.True(
            firstResponse.IsSuccessStatusCode);

        // Act
        var secondResponse =
            await auth.RegisterUserAsync(user);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            secondResponse.StatusCode);
    }

    [Fact]
    public async Task Login_WhenCredentialsAreValid_ReturnsSuccess()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var user =
            TestUserData.Customer();

        var registerResponse =
            await auth.RegisterUserAsync(user);

        Assert.True(
            registerResponse.IsSuccessStatusCode);

        // Act
        var loginResponse =
            await auth.LoginUserAsync(user);

        // Assert
        Assert.True(
            loginResponse.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Login_WhenEmailDoesNotExist_ReturnsUnauthorized()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var user =
            TestUserData.Customer();

        // Act
        var response =
            await auth.LoginUserAsync(user);

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task GetCurrentUser_WhenUserIsLoggedIn_ReturnsCurrentUser()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var user =
            TestUserData.Customer();

        var registerResponse =
            await auth.RegisterUserAsync(user);

        Assert.True(
            registerResponse.IsSuccessStatusCode);

        var loginResponse =
            await auth.LoginUserAsync(user);

        Assert.True(
            loginResponse.IsSuccessStatusCode);

        // Act
        var response =
            await client.GetAsync(
                "/auth/me");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var currentUser =
            await response.Content
                .ReadFromJsonAsync<UserDto>();

        Assert.NotNull(
            currentUser);

        Assert.False(
            string.IsNullOrWhiteSpace(currentUser.Id));

        Assert.Equal(
            user.UserName,
            currentUser.UserName);

        Assert.Equal(
            user.Email,
            currentUser.Email);
    }

    [Fact]
    public async Task GetCurrentUser_WhenUserIsNotLoggedIn_ReturnsUnauthorized()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        // Act
        var response =
            await client.GetAsync("/auth/me");

        // Assert
        var content =
            await response.Content.ReadAsStringAsync();

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task Logout_WhenUserIsLoggedIn_ReturnsSuccess()
    {
        // Arrange
        using var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var user =
            TestUserData.Customer();

        var loginResponse =
            await auth.RegisterAndLoginAsync(user);

        Assert.True(
            loginResponse.IsSuccessStatusCode);

        // Act
        var logoutResponse =
            await auth.LogoutUserAsync();

        // Assert
        Assert.True(
            logoutResponse.IsSuccessStatusCode);

        var currentUserResponse =
            await client.GetAsync(
                "/auth/me");

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            currentUserResponse.StatusCode);
    }
}
