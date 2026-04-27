using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Interfaces;

public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, Result>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<UpdateGenreCommandHandler> _logger;

    public UpdateGenreCommandHandler(IDbContext dbContext, ILogger<UpdateGenreCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(
        UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating genre {GenreId} with new name {GenreName}", request.Id, request.Name);
        var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Id == request.Id);
        if (genre == null)
        {
            _logger.LogWarning("Genre {GenreId} not found", request.Id);
            return Result.Fail($"Genre {request.Id} not found.");
        }

        if (await _dbContext.Genres.AnyAsync(g => g.Name == request.Name && g.Id != request.Id))
        {
            _logger.LogWarning("Genre {GenreName} already exists", request.Name);
            return Result.Fail($"Genre '{request.Name}' already exists.");
        }
        genre.Name = request.Name;
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Genre {GenreId} updated successfully", request.Id);
        return Result.Ok();
    }
}