using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Interfaces;
public class AssignMovieGenreCommandHandler : IRequestHandler<AssignMovieGenreCommand, Result>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<AssignMovieGenreCommandHandler> _logger;

    public AssignMovieGenreCommandHandler(IDbContext dbContext, ILogger<AssignMovieGenreCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(
        AssignMovieGenreCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Assigning genre {GenreId} to movie {MovieId}", request.GenreId, request.MovieId);

        var movie = await _dbContext.Movies.Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == request.MovieId);
        if (movie == null)
        {
            _logger.LogWarning("Movie {MovieId} not found", request.MovieId);
            return Result.Fail($"Movie {request.MovieId} not found.");
        }
         
        var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Id == request.GenreId);

        if (genre == null)
        {
            _logger.LogWarning("Genre {GenreId} not found", request.GenreId);
            return Result.Fail($"Genre {request.GenreId} not found.");
        }

        if (!movie.Genres.Any(g => g.Id == request.GenreId))
        {
            movie.Genres.Add(genre);
        }
        else
        {
            _logger.LogWarning("Genre already assigned {GenreId} to movie {MovieId}", request.GenreId, request.MovieId);
            return Result.Fail("Genre already assigned to movie.");
        }

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Genre {GenreId} assigned to movie {MovieId}", request.GenreId, request.MovieId);
        return Result.Ok();
    }
}