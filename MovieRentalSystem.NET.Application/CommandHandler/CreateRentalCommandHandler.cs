using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Domain.Entities;

public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, Result<RentalDto>>
{
    private readonly IDbContext _dbContext;

    public CreateRentalCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<RentalDto>> Handle(
        CreateRentalCommand request, CancellationToken cancellationToken)
    {
        if (!await _dbContext.Users.AnyAsync(u => u.Id == request.UserId))
            return Result.Fail<RentalDto>($"User with Id {request.UserId} does not exist.");

        if (!await _dbContext.MoviePhysicalCopies.AnyAsync(c => c.Id == request.MoviePhysicalCopyId))
            return Result.Fail<RentalDto>($"MoviePhysicalCopy with Id {request.MoviePhysicalCopyId} does not exist.");

        var rental = new Rental
        {
            UserId = request.UserId,
            MoviePhysicalCopyId = request.MoviePhysicalCopyId,
            RentalStartDate = request.RentalStartDate,
            DueDate = request.DueDate
        };

        _dbContext.Rentals.Add(rental);
        await _dbContext.SaveChangesAsync();

        rental = await _dbContext.Rentals
            .Include(r => r.User)
            .Include(r => r.MoviePhysicalCopy)
                .ThenInclude(m => m.Movie)
            .FirstOrDefaultAsync(r => r.Id == request.UserId);
        if (rental == null)
            return Result.Fail($"Rental with ID {request.UserId} not found.");
        return Result.Ok(rental.MapToRentalDto());
    }
}