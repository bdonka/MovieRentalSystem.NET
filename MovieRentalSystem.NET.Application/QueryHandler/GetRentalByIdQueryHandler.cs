using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetRentalByIdQueryHandler : IRequestHandler<GetRentalByIdQuery, Result<RentalDto>>
{
    private readonly IDbContext _dbContext;
    public GetRentalByIdQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<RentalDto>> Handle(
        GetRentalByIdQuery request, CancellationToken cancellationToken)
    {
        var rental = await _dbContext.Rentals
                    .Include(r => r.User)
                    .Include(r => r.MoviePhysicalCopy)
                        .ThenInclude(m => m.Movie)
                    .FirstOrDefaultAsync(r => r.Id == request.Id);
        if (rental == null)
            return Result.Fail($"Rental with ID {request.Id} not found.");
        return Result.Ok(rental.MapToRentalDto());
    }
}