using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetRentalQueryHandler : IRequestHandler<GetRentalQuery, IEnumerable<RentalDto>>
{
    private readonly IDbContext _dbContext;
    public GetRentalQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<RentalDto>> Handle(
        GetRentalQuery request, CancellationToken cancellationToken)
    {
        var rentals = await _dbContext.Rentals
            .Include(r => r.User)
            .Include(r => r.MoviePhysicalCopy)
                .ThenInclude(m => m.Movie)
            .ToListAsync();
        return rentals.Select(r => r.MapToRentalDto()).ToList();
    }
}