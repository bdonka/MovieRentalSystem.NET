using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Interfaces;
public class AssignMovieGenreCommandHandler : IRequestHandler<AssignMovieGenreCommand, Result>
{
    private readonly IDbContext _dbContext;

    public AssignMovieGenreCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(
        AssignMovieGenreCommand request, CancellationToken cancellationToken)
    {
        var movie = await _dbContext.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == request.MovieId);
        if (movie == null)
            return Result.Fail($"Movie with Id {request.MovieId} not found.");

        var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Id == request.GenreId);

        if (genre == null)
            return Result.Fail($"Genre with Id {request.GenreId} not found.");

        if (!movie.Genres.Any(g => g.Id == request.GenreId))
            movie.Genres.Add(genre);

        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
}