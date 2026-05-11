using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Interfaces;

public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, Result>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<UpdateMovieCommandHandler> _logger;

    public UpdateMovieCommandHandler(IDbContext dbContext, ILogger<UpdateMovieCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(
        UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating movie {MovieId}", request.Id);
        var movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Id == request.Id);
        if (movie == null)
        {
            _logger.LogInformation("Movie {MovieId} not found", request.Id);
            return Result.Fail(new MovieNotFoundError(request.Id));
        }

        if (await _dbContext.Movies.AnyAsync(m => m.Title == request.Title && m.Id != request.Id))
        {
            _logger.LogInformation("Movie {MovieTitle} already exists", request.Title);
            return Result.Fail(new MovieAlreadyExistsError(request.Title));
        }

        movie.Title = request.Title;
        movie.Description = request.Description;
        movie.ReleaseYear = request.ReleaseYear;
        movie.RentalPrice = request.RentalPrice;

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Movie {MovieId} updated successfully", request.Id);
        return Result.Ok();
    }
}