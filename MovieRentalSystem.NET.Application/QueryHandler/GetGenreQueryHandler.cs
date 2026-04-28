using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetGenreQueryHandler : IRequestHandler<GetGenreQuery, IEnumerable<GenreDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<GetGenreQueryHandler> _logger;
    public GetGenreQueryHandler(IDbContext dbContext, ILogger<GetGenreQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<GenreDto>> Handle(
        GetGenreQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all genres");
        var genres = await _dbContext.Genres.ToListAsync();
        _logger.LogInformation("Genres got successfully");
        return genres.Select(g => g.MapToGenreDto()).ToList();
    }
}