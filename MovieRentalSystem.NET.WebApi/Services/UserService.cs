using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.WebApi.Data;
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
    private readonly ApplicationDbContext _context;
    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = await _context.Users.ToListAsync();
        return users.Select(u => u.MapToUserResponse());
    }

    public async Task<UserResponse?> GetByIdAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user?.MapToUserResponse();
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            throw new InvalidOperationException($"User with Email '{request.Email}' already exists.");

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = HashPassword(request.Password),
            Role = "User"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user.MapToUserResponse();
    }

    public async Task<bool> UpdateAsync(int id, UpdateUserRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return false;

        if (await _context.Users.AnyAsync(u => u.Email == request.Email && u.Id != id))
            throw new InvalidOperationException($"User with Email '{request.Email}' already exists.");

        user.Name = request.Name;
        user.Email = request.Email;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
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