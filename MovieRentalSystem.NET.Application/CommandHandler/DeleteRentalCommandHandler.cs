using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Interfaces;

public class DeleteRentalCommandHandler : IRequestHandler<DeleteRentalCommand, Result>
{
    private readonly IDbContext _dbContext;

    public DeleteRentalCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(
        DeleteRentalCommand request, CancellationToken cancellationToken)
    {
        var rental = await _dbContext.Rentals.FirstOrDefaultAsync(r => r.Id == request.Id);
        if (rental == null)
            return Result.Fail($"Rental with ID {request.Id} not found.");

        _dbContext.Rentals.Remove(rental);
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
}