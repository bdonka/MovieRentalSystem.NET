using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IDbContext _dbContext;

    public UpdateUserCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(
        UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
        if (user == null)
            return Result.Fail($"User with ID {request.Id} not found.");

        if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email && u.Id != request.Id))
            return Result.Fail($"User with Email '{request.Email}' already exists.");

        user.Name = request.Name;
        user.Email = request.Email;

        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
}