using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Interfaces;

public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, Result>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<DeleteMovieCommandHandler> _logger;

    public DeleteMovieCommandHandler(IDbContext dbContext, ILogger<DeleteMovieCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting movie {MovieId}", request.Id);

        var movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Id == request.Id);
        if (movie == null)
        {
            _logger.LogWarning("Movie {MovieId} not found", request.Id);
            return Result.Fail($"Movie {request.Id} not found.");
        }

        _dbContext.Movies.Remove(movie);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Movie {MovieId} deleted successfully", request.Id);
        return Result.Ok();
    }
}