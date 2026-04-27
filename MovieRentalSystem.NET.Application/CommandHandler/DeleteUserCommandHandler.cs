using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Interfaces;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IDbContext _dbContext;

    public DeleteUserCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(
        DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.Include(u => u.Rentals).FirstOrDefaultAsync(u => u.Id == request.Id);
        if (user == null)
            return Result.Fail($"User with ID {request.Id} not found.");
        if (user.Rentals.Count != 0)
            return Result.Fail($"User has assigned 1 or more rentals.");
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
}