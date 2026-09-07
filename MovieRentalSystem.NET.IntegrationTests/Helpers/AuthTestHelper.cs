using System.Net.Http.Json;
using MovieRentalSystem.NET.Application.Dtos;

namespace MovieRentalSystem.NET.IntegrationTests.Helpers;

public class AuthTestHelper
{
    private readonly HttpClient _client;

    public AuthTestHelper(
        HttpClient client)
    {
        _client = client;
    }

    public async Task<HttpResponseMessage> RegisterUserAsync(
        TestUserData user)
    {
        var request = new
        {
            user.UserName,
            user.Email,
            user.Password
        };

        return await _client.PostAsJsonAsync(
            "/auth/register",
            request);
    }

    public async Task<HttpResponseMessage> LoginUserAsync(
        TestUserData user)
    {
        var request = new
        {
            user.Email,
            user.Password,
            RememberMe = false
        };

        var response =
            await _client.PostAsJsonAsync(
                "/auth/login",
                request);

        var responseBody =
            await response.Content.ReadAsStringAsync();

        foreach (var header in response.Headers)
        {
            Console.WriteLine(
                $"LOGIN HEADER: {header.Key} = {string.Join(", ", header.Value)}");
        }

        return response;
    }

    public async Task<HttpResponseMessage> RegisterAndLoginAsync(
        TestUserData user)
    {
        var registerResponse =
            await RegisterUserAsync(user);

        if (!registerResponse.IsSuccessStatusCode)
        {
            return registerResponse;
        }

        return await LoginUserAsync(user);
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

    public async Task<HttpResponseMessage> LogoutUserAsync()
    {
        return await _client.PostAsync(
            "/auth/logout",
            content: null);
    }

    public async Task<HttpResponseMessage> DeleteCurrentUserAsync()
    {
        var currentUser =
            await GetCurrentUserAsync();

        if (currentUser is null)
        {
            throw new InvalidOperationException(
                "There is no authenticated user to delete.");
        }

        return await _client.DeleteAsync(
            $"/api/users/{currentUser.Id}");
    }
}
