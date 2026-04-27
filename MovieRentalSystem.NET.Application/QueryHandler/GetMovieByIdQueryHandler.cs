using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, Result<MovieDto>>
{
    private readonly IDbContext _dbContext;
    public GetMovieByIdQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<MovieDto>> Handle(
        GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        var movie = await _dbContext.Movies
            .Include(m => m.Genres)
            .Include(m => m.PhysicalCopies)
            .FirstOrDefaultAsync(m => m.Id == request.Id);
        if (movie == null)
            return Result.Fail<MovieDto>($"Movie with ID {request.Id} not found.");
        return Result.Ok(movie.MapToMovieDto());
    }
}