using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Common.Errors;
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

        if (movie == null)
        {
            _logger.LogWarning("Movie {MovieId} not found", request.MovieId);
            return Result.Fail(new MovieNotFoundError(request.MovieId));
        }

        var assignedGenre = movie.Genres.FirstOrDefault(g => g.Id == request.GenreId);
        if (assignedGenre == null)
        {
            _logger.LogInformation("Genre {GenreId} is not assigned to movie {MovieId}", request.GenreId, request.MovieId);
            return Result.Ok();
        }

        movie.Genres.Remove(assignedGenre);

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Genre {GenreId} removed from movie {MovieId}", request.GenreId, request.MovieId);
        return Result.Ok();
    }
}