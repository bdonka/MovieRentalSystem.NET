using FluentResults;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Infrastructure.Data;
using MovieRentalSystem.NET.Infrastructure.Mapping;
using System.Security.Cryptography;
using System.Text;

namespace MovieRentalSystem.NET.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _context.Users
            .Include(u => u.Rentals)
            .ThenInclude(r => r.MoviePhysicalCopy)
            .ThenInclude(m => m.Movie)
            .ToListAsync();
        var result = users.Select(u => u.MapToUserDto()).ToList();
        return result;
    }

    public async Task<Result<UserDto>> GetByIdAsync(int id)
    {
        var user = await _context.Users.Include(u => u.Rentals).FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) 
            return Result.Fail<UserDto>($"User with ID {id} not found.");
        return Result.Ok(user.MapToUserDto());
    }

    public async Task<Result<UserDto>> CreateAsync(CreateUserDto request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            return Result.Fail<UserDto>($"User with Email '{request.Email}' already exists.");

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = HashPassword(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Result.Ok(user.MapToUserDto());
    }

    public async Task<Result> UpdateAsync(int id, UserDto request)
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