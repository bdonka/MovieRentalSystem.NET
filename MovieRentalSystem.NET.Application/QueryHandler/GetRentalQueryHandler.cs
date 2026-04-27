using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetRentalQueryHandler : IRequestHandler<GetRentalQuery, IEnumerable<RentalDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<GetRentalQueryHandler> _logger;
    public GetRentalQueryHandler(IDbContext dbContext, ILogger<GetRentalQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<RentalDto>> Handle(
        GetRentalQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all rentals");
        var rentals = await _dbContext.Rentals
            .Include(r => r.User)
            .Include(r => r.MoviePhysicalCopy)
                .ThenInclude(m => m.Movie)
            .ToListAsync();
        var results = rentals.Select(r => r.MapToRentalDto()).ToList();

        _logger.LogInformation("Retrieved {Count} rentals", results.Count);
        return results;
    }
}