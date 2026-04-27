using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    private readonly IDbContext _dbContext;

    public CreateUserCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<UserDto>> Handle(
        CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email))
            return Result.Fail<UserDto>($"User with Email '{request.Email}' already exists.");

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = HashPassword(request.Password)
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return Result.Ok(user.MapToUserDto());
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();

        var bytes = Encoding.UTF8.GetBytes(password);

        var hash = sha256.ComputeHash(bytes);

        return Convert.ToBase64String(hash);
    }
}