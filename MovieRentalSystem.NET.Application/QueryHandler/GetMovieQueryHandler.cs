using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetMovieQueryHandler : IRequestHandler<GetMovieQuery, IEnumerable<MovieDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<GetMovieQueryHandler> _logger;
    public GetMovieQueryHandler(IDbContext dbContext, ILogger<GetMovieQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<MovieDto>> Handle(
        GetMovieQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all movies");
        var movies = await _dbContext.Movies
            .Include(m => m.Genres)
            .Include(m => m.PhysicalCopies)
            .ToListAsync();

        var result = movies.Select(m => m.MapToMovieDto()).ToList();

        _logger.LogInformation("Movies got successfully, count: {Count}", result.Count);

        return result;
    }
}