using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetMovieQueryHandler : IRequestHandler<GetMovieQuery, PagedResponse<MovieDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<GetMovieQueryHandler> _logger;
    public GetMovieQueryHandler(IDbContext dbContext, ILogger<GetMovieQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<PagedResponse<MovieDto>> Handle(
        GetMovieQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all movies with PageNumber={PageNumber}, PageSize={PageSize}", request.PageNumber, request.PageSize);
        var query = _dbContext.Movies
            .Include(m => m.Genres)
            .Include(m => m.PhysicalCopies)
            .AsQueryable();

        var totalRecords = await query.CountAsync();

        var movies = await query
            .ApplyPagination(request.PageNumber, request.PageSize)
            .ToListAsync();

        var results = movies.Select(m => m.MapToMovieDto()).ToList();
        _logger.LogInformation("Retrieved {Count} genres (PageNumber={PageNumber}, PageSize={PageSize}, TotalRecords={TotalRecords})",
            results.Count,
            request.PageNumber,
            request.PageSize,
            totalRecords);

        return new PagedResponse<MovieDto>(results, request.PageNumber, request.PageSize, totalRecords);
    }
}