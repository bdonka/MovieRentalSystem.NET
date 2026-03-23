using MovieRentalSystem.NET.WebApi.Entities;
using MovieRentalSystem.NET.WebApi.Mappings;
using MovieRentalSystem.NET.WebApi.Models.Requests.Users;
using MovieRentalSystem.NET.WebApi.Models.Responses;
using MovieRentalSystem.NET.WebApi.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace MovieRentalSystem.NET.WebApi.Services;

public class UserService : IUserService
{
    private static readonly List<User> _users = new();

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        return _users.Select(u => u.MapToUserResponse());
    }

    public async Task<UserResponse?> GetByIdAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return user?.MapToUserResponse();
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        var user = new User
        {
            Id = _users.Count + 1,
            Name = request.Name,
            Email = request.Email,
            Password = HashPassword(request.Password),
            Role = "User"
        };

        _users.Add(user);
        return user.MapToUserResponse();
    }

    public async Task<bool> UpdateAsync(int id, UpdateUserRequest request)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return false;

        user.Name = request.Name;
        user.Email = request.Email;

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return false;

        _users.Remove(user);
        return true;
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();

        var bytes = Encoding.UTF8.GetBytes(password);

        var hash = sha256.ComputeHash(bytes);

        return Convert.ToBase64String(hash);
    }
}