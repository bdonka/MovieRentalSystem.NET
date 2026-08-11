using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.WebApi.Models.Requests.Users;

namespace MovieRentalSystem.NET.IntegrationTests.Helpers;

public class AuthTestHelper
{
    private readonly HttpClient _client;

    public AuthTestHelper(HttpClient client)
    {
        _client =
            client ??
            throw new ArgumentNullException(nameof(client));
    }

    public async Task<HttpResponseMessage> RegisterUserAsync(
        TestUserData userData)
    {
        var request = new RegisterRequest
        {
            UserName = userData.UserName,
            Email = userData.Email,
            Password = userData.Password
        };

        return await PostJsonAsync(
            "/auth/register",
            request);
    }

    public async Task<HttpResponseMessage> LoginUserAsync(
        TestUserData userData,
        bool rememberMe = false)
    {
        return await LoginUserAsync(
            userData.Email,
            userData.Password,
            rememberMe);
    }

    public async Task<HttpResponseMessage> LoginUserAsync(
        string email,
        string password,
        bool rememberMe = false)
    {
        var request = new LoginRequest
        {
            Email = email,
            Password = password,
            RememberMe = rememberMe
        };

        return await PostJsonAsync(
            "/auth/login",
            request);
    }

    public async Task<HttpResponseMessage> LogoutUserAsync()
    {
        return await _client.PostAsync(
            "/auth/logout",
            null);
    }

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        var response =
            await _client.GetAsync("/auth/me");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content
            .ReadFromJsonAsync<UserDto>();
    }

    public async Task<HttpResponseMessage> GetAsync(
        string url)
    {
        return await _client.GetAsync(url);
    }

    public async Task<HttpResponseMessage> RegisterAndLoginAsync(
        TestUserData userData)
    {
        var register =
            await RegisterUserAsync(userData);

        if (!register.IsSuccessStatusCode)
        {
            return register;
        }

        return await LoginUserAsync(userData);
    }

    private async Task<HttpResponseMessage> PostJsonAsync<T>(
        string url,
        T body)
    {
        using var content =
            new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json");

        return await _client.PostAsync(
            url,
            content);
    }

    public static bool IsSuccessful(
        HttpResponseMessage response)
    {
        return response.IsSuccessStatusCode;
    }

    public static bool IsUnauthorized(
        HttpResponseMessage response)
    {
        return response.StatusCode ==
               HttpStatusCode.Unauthorized;
    }
}
