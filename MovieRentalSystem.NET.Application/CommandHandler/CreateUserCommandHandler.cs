using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(IDbContext dbContext, ILogger<CreateUserCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<UserDto>> Handle(
        CreateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating user with email: {UserEmail}", request.Email);

        if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email))
        {
            _logger.LogWarning("User already exists with email: {UserEmail}", request.Email);
            return Result.Fail<UserDto>($"User with Email '{request.Email}' already exists.");
        }

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = HashPassword(request.Password)
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("User {UserId} created successfully", user.Id);
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