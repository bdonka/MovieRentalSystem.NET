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
    public async Task Register_WithValidData_ReturnsOk()
    {
        // Arrange
        var client =
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
    public async Task Register_WithExistingEmail_ReturnsBadRequest()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var user =
            TestUserData.Customer();

        await auth.RegisterUserAsync(user);

        // Act
        var response =
            await auth.RegisterUserAsync(user);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOk()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var user =
            TestUserData.Customer();

        await auth.RegisterUserAsync(user);

        // Act
        var response =
            await auth.LoginUserAsync(user);

        // Assert
        Assert.True(
            response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Login_WithUnknownEmail_ReturnsUnauthorized()
    {
        // Arrange
        var client =
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
    public async Task Me_AfterLogin_ReturnsCurrentUser()
    {
        // Arrange
        var client =
            _fixture.CreateClient();

        var auth =
            _fixture.CreateAuthHelper(client);

        var user =
            TestUserData.Customer();

        var register =
            await auth.RegisterUserAsync(user);

        Assert.True(
            register.IsSuccessStatusCode);

        var login =
            await auth.LoginUserAsync(user);

        Assert.True(
            login.IsSuccessStatusCode);

        // Act
        var response =
            await client.GetAsync(
                "/auth/me");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var body =
            await response.Content
                .ReadFromJsonAsync<UserDto>();

        Assert.NotNull(body);

        Assert.Equal(
            user.Email,
            body.Email);
    }

    [Fact]
    public async Task Me_WithoutLogin_ReturnsUnauthorized()
    {
        // Arrange
        var client =
            _fixture.CreateAnonymousClient();

        // Act
        var response =
            await client.GetAsync(
                "/auth/me");

        // Assert
        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task Logout_AfterLogin_ReturnsOk()
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
            await auth.LogoutUserAsync();

        // Assert
        Assert.True(
            response.IsSuccessStatusCode);
    }
}
