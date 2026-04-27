using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(IDbContext dbContext, ILogger<UpdateUserCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(
        UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User {UserId} updated", request.Id);

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
        if (user == null)
        {
            _logger.LogWarning("User {UserId} not found", request.Id);
            return Result.Fail($"User {request.Id} not found.");
        }

        if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email && u.Id != request.Id))
        {
            _logger.LogWarning("User with Email: {UserEmail} already exists", request.Email);
            return Result.Fail($"User with Email '{request.Email}' already exists.");
        }

        user.Name = request.Name;
        user.Email = request.Email;

        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("User {UserId} updated successfully", request.Id);
        return Result.Ok();
    }
}