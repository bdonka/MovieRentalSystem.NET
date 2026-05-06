using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetMoviePhysicalCopyQueryHandler : IRequestHandler<GetMoviePhysicalCopyQuery, PagedResponse<MoviePhysicalCopyDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<GetMoviePhysicalCopyQueryHandler> _logger;
    public GetMoviePhysicalCopyQueryHandler(IDbContext dbContext, ILogger<GetMoviePhysicalCopyQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<PagedResponse<MoviePhysicalCopyDto>> Handle(
        GetMoviePhysicalCopyQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all movie physical copies with PageNumber={PageNumber}, PageSize={PageSize}", request.PageNumber, request.PageSize);
        var query = _dbContext.MoviePhysicalCopies
            .Include(m => m.Movie)
            .AsQueryable();

        var totalRecords = await query.CountAsync();

        var copies = await query
            .ApplyPagination(request.PageNumber, request.PageSize)
            .ToListAsync();

        var results = copies.Select(c => c.MapToMoviePhysicalCopyDto()).ToList();
        _logger.LogInformation("Retrieved { Count} movie physical copies (PageNumber ={ PageNumber}, PageSize ={ PageSize}, TotalRecords ={ TotalRecords})",
            results.Count,
            request.PageNumber,
            request.PageSize,
            totalRecords);
        return new PagedResponse<MoviePhysicalCopyDto>(results, request.PageNumber, request.PageSize, totalRecords);
    }
}