using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Domain.Entities;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly UserManager<User> _userManager;
    private readonly IDbContext _dbContext;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(UserManager<User> userManager, IDbContext dbContext, ILogger<DeleteUserCommandHandler> logger)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeleteUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting user {UserId}", request.Id);

        var user = await _userManager.FindByIdAsync(request.Id);
        if (user == null)
        {
            _logger.LogWarning("User {UserId} not found", request.Id);
            return Result.Fail(new UserNotFoundError(request.Id));
        }

        var hasRentals = await _dbContext.Rentals
            .AnyAsync(r => r.UserId == user.Id, cancellationToken);

        if (hasRentals)
        {
            _logger.LogWarning(
                "User {UserId} has assigned rentals",
                request.Id);

            return Result.Fail(new UserHasAssignedRentalsError(request.Id));
        }

        var result = await _userManager.DeleteAsync(user);

        _logger.LogInformation("User {UserId} deleted successfully", request.Id);
        return Result.Ok();
    }
}