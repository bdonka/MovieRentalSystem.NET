using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Interfaces;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(IDbContext dbContext, ILogger<DeleteUserCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeleteUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting user {UserId}", request.Id);

        var user = await _dbContext.Users.Include(u => u.Rentals).FirstOrDefaultAsync(u => u.Id == request.Id);
        if (user == null)
        {
            _logger.LogWarning("User {UserId} not found", request.Id);
            return Result.Fail($"User {request.Id} not found.");
        }
        if (user.Rentals.Count != 0)
        {
            _logger.LogWarning("User {UserId} has assigned 1 or more rentals", request.Id);
            return Result.Fail($"User has assigned 1 or more rentals.");
        }
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("User {UserId} deleted successfully", request.Id);
        return Result.Ok();
    }
}