using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetRentalQueryHandler : IRequestHandler<GetRentalQuery, PagedResponse<RentalDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<GetRentalQueryHandler> _logger;
    public GetRentalQueryHandler(IDbContext dbContext, ILogger<GetRentalQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<PagedResponse<RentalDto>> Handle(
        GetRentalQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all rentals with PageNumber={PageNumber}, PageSize={PageSize}", request.PageNumber, request.PageSize);
        var query = _dbContext.Rentals
            .Include(r => r.User)
            .Include(r => r.MoviePhysicalCopy)
                .ThenInclude(m => m.Movie)
            .AsQueryable();

        var totalRecords = await query.CountAsync();

        var rentals = await query
            .ApplyPagination(request.PageNumber, request.PageSize)
            .ToListAsync();

        var results = rentals.Select(r => r.MapToRentalDto()).ToList();
        _logger.LogInformation("Retrieved {Count} rentals (PageNumber={PageNumber}, PageSize={PageSize}, TotalRecords={TotalRecords})",
            results.Count,
            request.PageNumber,
            request.PageSize,
            totalRecords);

        return new PagedResponse<RentalDto>(results, request.PageNumber, request.PageSize, totalRecords);
    }
}