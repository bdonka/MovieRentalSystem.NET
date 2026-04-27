using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetGenreQueryHandler : IRequestHandler<GetGenreQuery, IEnumerable<GenreDto>>
{
    private readonly IDbContext _dbContext;
    public GetGenreQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<GenreDto>> Handle(
        GetGenreQuery request, CancellationToken cancellationToken)
    {
        var genres = await _dbContext.Genres.ToListAsync();
        return genres.Select(g => g.MapToGenreDto()).ToList();
    }
}