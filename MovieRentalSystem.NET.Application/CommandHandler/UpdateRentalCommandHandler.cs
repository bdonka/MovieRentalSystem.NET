using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Domain.Enums;

public class UpdateRentalCommandHandler : IRequestHandler<UpdateRentalCommand, Result>
{
    private readonly IDbContext _dbContext;

    public UpdateRentalCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(
        UpdateRentalCommand request, CancellationToken cancellationToken)
    {
        var rental = await _dbContext.Rentals.FirstOrDefaultAsync(r => r.Id == request.Id);
        if (rental == null)
            return Result.Fail($"Rental with ID {request.Id} not found.");

        rental.RentalStartDate = request.RentalStartDate;
        rental.DueDate = request.DueDate;
        rental.ReturnDate = request.ReturnDate;
        rental.TotalPrice = request.TotalPrice;
        rental.Status = Enum.Parse<RentalStatus>(request.Status.ToString());

        if (request.ReturnDate != null)
        {
            var copy = await _dbContext.MoviePhysicalCopies
                .FirstOrDefaultAsync(c => c.Id == rental.MoviePhysicalCopyId);

            if (copy != null)
                copy.Status = MovieCopyStatus.Available;
        }

        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
}