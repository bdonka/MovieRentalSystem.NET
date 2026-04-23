using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Interfaces;
public class RemoveMovieGenreCommandHandler : IRequestHandler<RemoveMovieGenreCommand, Result>
{
    private readonly IDbContext _dbContext;

    public RemoveMovieGenreCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(
        RemoveMovieGenreCommand request, CancellationToken cancellationToken)
    {
        var movie = await _dbContext.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == request.MovieId);
        var genre = await _dbContext.Genres.FindAsync(request.GenreId);

        if (movie == null)
            return Result.Fail($"Movie with Id {request.MovieId} not found.");

        if (genre == null)
            return Result.Fail($"Genre with Id {request.GenreId} not found.");

        if (movie.Genres.Contains(genre))
            movie.Genres.Remove(genre);

        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
}