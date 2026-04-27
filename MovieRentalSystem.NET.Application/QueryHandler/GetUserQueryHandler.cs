using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, IEnumerable<UserDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<GetUserQueryHandler> _logger;
    public GetUserQueryHandler(IDbContext dbContext, ILogger<GetUserQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<UserDto>> Handle(
        GetUserQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all users");
        var users = await _dbContext.Users
                    .Include(u => u.Rentals)
                    .ThenInclude(r => r.MoviePhysicalCopy)
                    .ThenInclude(m => m.Movie)
                    .ToListAsync();
        var result = users.Select(u => u.MapToUserDto()).ToList();
        _logger.LogInformation("Retrieved {UserCount} users", result.Count);
        return result;
    }
}