using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetMoviePhysicalCopyQueryHandler : IRequestHandler<GetMoviePhysicalCopyQuery, IEnumerable<MoviePhysicalCopyDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<GetMoviePhysicalCopyQueryHandler> _logger;
    public GetMoviePhysicalCopyQueryHandler(IDbContext dbContext, ILogger<GetMoviePhysicalCopyQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<MoviePhysicalCopyDto>> Handle(
        GetMoviePhysicalCopyQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all movie physical copies");
        var copies = await _dbContext.MoviePhysicalCopies
            .Include(m => m.Movie)
            .ToListAsync();
        _logger.LogInformation("Retrieved {Count} movie physical copies", copies.Count);
        return copies.Select(c => c.MapToMoviePhysicalCopyDto()).ToList();
    }
}