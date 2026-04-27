using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Interfaces;
public class RemoveMovieGenreCommandHandler : IRequestHandler<RemoveMovieGenreCommand, Result>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<RemoveMovieGenreCommandHandler> _logger;

    public RemoveMovieGenreCommandHandler(IDbContext dbContext, ILogger<RemoveMovieGenreCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(
        RemoveMovieGenreCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Removing genre {GenreId} from movie {MovieId}", request.GenreId, request.MovieId);

        var movie = await _dbContext.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == request.MovieId);
        var genre = await _dbContext.Genres.FindAsync(request.GenreId);

        if (movie == null)
        {
            _logger.LogWarning("Movie {MovieId} not found", request.MovieId);
            return Result.Fail($"Movie {request.MovieId} not found.");
        }

        if (genre == null)
        {
            _logger.LogWarning("Genre {GenreId} not found", request.GenreId);
            return Result.Fail($"Genre {request.GenreId} not found.");
        }

        if (movie.Genres.Contains(genre))
        {
            _logger.LogWarning("Genre {GenreId} is already assigned to movie {MovieId}", request.GenreId, request.MovieId); 
            movie.Genres.Remove(genre);
        }

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Genre {GenreId} removed from movie {MovieId}", request.GenreId, request.MovieId);
        return Result.Ok();
    }
}