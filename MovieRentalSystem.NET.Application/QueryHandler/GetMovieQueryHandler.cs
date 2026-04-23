using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetMovieQueryHandler : IRequestHandler<GetMovieQuery, IEnumerable<MovieDto>>
{
    private readonly IDbContext _dbContext;
    public GetMovieQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<MovieDto>> Handle(
        GetMovieQuery request, CancellationToken cancellationToken)
    {
        var movies = await _dbContext.Movies
            .Include(m => m.Genres)
            .Include(m => m.PhysicalCopies)
            .ToListAsync();

        var result = movies.Select(m => m.MapToMovieDto()).ToList();

        return result;
    }
}