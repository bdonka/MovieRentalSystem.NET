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
        var pageNumber = request.PageNumber;
        var pageSize = request.PageSize;

        _logger.LogInformation("Getting all genres");
        var genres = await _dbContext.Genres
            .AsQueryable()
            .ApplyPagination(pageNumber, pageSize)
            .ToListAsync();
        var totalRecords = await _dbContext.Genres.CountAsync();

        _logger.LogInformation("Genres got successfully");
        var result = genres.Select(g => g.MapToGenreDto()).ToList();
        return new PagedResponse<GenreDto>(result, pageNumber, pageSize, totalRecords);
    }
}