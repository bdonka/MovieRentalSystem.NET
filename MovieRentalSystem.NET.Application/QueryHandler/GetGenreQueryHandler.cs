using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetGenreQueryHandler : IRequestHandler<GetGenreQuery, PagedResponse<GenreDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<GetGenreQueryHandler> _logger;
    public GetGenreQueryHandler(IDbContext dbContext, ILogger<GetGenreQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<PagedResponse<GenreDto>> Handle(
        GetGenreQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all genres with PageNumber={PageNumber}, PageSize={PageSize}", request.PageNumber, request.PageSize);
        var query = _dbContext.Genres
            .AsQueryable();

        var totalRecords = await query.CountAsync();

        var genres = await query
            .ApplyPagination(request.PageNumber, request.PageSize)
            .ToListAsync();

        var results = genres.Select(g => g.MapToGenreDto()).ToList();

        _logger.LogInformation("Retrieved {Count} genres (PageNumber={PageNumber}, PageSize={PageSize}, TotalRecords={TotalRecords})",
            results.Count,
            request.PageNumber,
            request.PageSize,
            totalRecords);
        return new PagedResponse<GenreDto>(results, request.PageNumber, request.PageSize, totalRecords);
    }
}