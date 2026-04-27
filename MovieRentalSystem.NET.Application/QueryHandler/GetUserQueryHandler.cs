using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, IEnumerable<UserDto>>
{
    private readonly IDbContext _dbContext;
    public GetUserQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<UserDto>> Handle(
        GetUserQuery request, CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users
                    .Include(u => u.Rentals)
                    .ThenInclude(r => r.MoviePhysicalCopy)
                    .ThenInclude(m => m.Movie)
                    .ToListAsync();
        var result = users.Select(u => u.MapToUserDto()).ToList();
        return result;
    }
}