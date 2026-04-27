using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Interfaces;

public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, Result>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<DeleteGenreCommandHandler> _logger;

    public DeleteGenreCommandHandler(IDbContext dbContext, ILogger<DeleteGenreCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting genre {GenreId}", request.Id);

        var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Id == request.Id);
        if (genre == null)
        {
            _logger.LogWarning("Genre {GenreId} not found", request.Id);
            return Result.Fail($"Genre {request.Id} not found.");
        }
        _dbContext.Genres.Remove(genre);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Genre {GenreId} deleted successfully", request.Id);

        return Result.Ok();
    }
}