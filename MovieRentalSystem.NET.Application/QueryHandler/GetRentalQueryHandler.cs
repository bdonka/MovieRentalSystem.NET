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
        var pageNumber = request.PageNumber;
        var pageSize = request.PageSize;

        _logger.LogInformation("Getting all rentals");
        var rentals = await _dbContext.Rentals
            .Include(r => r.User)
            .Include(r => r.MoviePhysicalCopy)
                .ThenInclude(m => m.Movie)
            .AsQueryable()
            .ApplyPagination(pageNumber, pageSize)
            .ToListAsync();
        var totalRecords = await _dbContext.Rentals.CountAsync();

        var results = rentals.Select(r => r.MapToRentalDto()).ToList();
        _logger.LogInformation("Retrieved {Count} rentals", results.Count);

        return new PagedResponse<RentalDto>(results, pageNumber, pageSize, totalRecords);
    }
}