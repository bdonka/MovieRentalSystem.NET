using FluentResults;
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

    public async Task<Result<UserResponse>> GetByIdAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) 
            return Result.Fail<UserResponse>($"User with ID {id} not found.");
        return Result.Ok(user.MapToUserResponse());
    }

    public async Task<Result<UserResponse>> CreateAsync(CreateUserRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            return Result.Fail<UserResponse>($"User with Email '{request.Email}' already exists.");

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = HashPassword(request.Password),
            Role = "User"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Result.Ok(user.MapToUserResponse());
    }

    public async Task<Result> UpdateAsync(int id, UpdateUserRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            return Result.Fail($"User with ID {id} not found.");

        if (await _context.Users.AnyAsync(u => u.Email == request.Email && u.Id != id))
            return Result.Fail($"User with Email '{request.Email}' already exists.");

        user.Name = request.Name;
        user.Email = request.Email;

        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            return Result.Fail($"User with ID {id} not found.");
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();

        var bytes = Encoding.UTF8.GetBytes(password);

        var hash = sha256.ComputeHash(bytes);

        return Convert.ToBase64String(hash);
    }
}