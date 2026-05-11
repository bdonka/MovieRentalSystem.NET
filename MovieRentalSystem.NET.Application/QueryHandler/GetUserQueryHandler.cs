using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, PagedResponse<UserDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<GetUserQueryHandler> _logger;
    public GetUserQueryHandler(IDbContext dbContext, ILogger<GetUserQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<PagedResponse<UserDto>> Handle(
        GetUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all users with PageNumber={PageNumber}, PageSize={PageSize}", request.PageNumber, request.PageSize);
        var query = _dbContext.Users
                    .Include(u => u.Rentals)
                    .ThenInclude(r => r.MoviePhysicalCopy)
                    .ThenInclude(m => m.Movie)
                    .AsQueryable();

        var totalRecords = await query.CountAsync();

        var users = await query
                    .ApplyPagination(request.PageNumber, request.PageSize)
                    .ToListAsync();

        var results = users.Select(u => u.MapToUserDto()).ToList();
        _logger.LogInformation("Retrieved {Count} users (PageNumber={PageNumber}, PageSize={PageSize}, TotalRecords={TotalRecords})",
            results.Count,
            request.PageNumber,
            request.PageSize,
            totalRecords);

        return new PagedResponse<UserDto>(results, request.PageNumber, request.PageSize, totalRecords);
    }
}