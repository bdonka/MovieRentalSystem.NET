using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Domain.Entities;

public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, Result<MovieDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<CreateMovieCommandHandler> _logger;

    public CreateMovieCommandHandler(IDbContext dbContext, ILogger<CreateMovieCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<MovieDto>> Handle(
        CreateMovieCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating a new movie {MovieTitle}", request.Title);

        if (await _dbContext.Movies.AnyAsync(m => m.Title == request.Title))
        {
            _logger.LogWarning("Movie already exists {MovieTitle}", request.Title);
            return Result.Fail<MovieDto>($"Movie '{request.Title}' already exists.");
        }

        var movie = new Movie
        {
            Title = request.Title,
            Description = request.Description,
            ReleaseYear = request.ReleaseYear,
            RentalPrice = request.RentalPrice
        };

        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync();
        movie = await _dbContext.Movies
            .Include(m => m.Genres)
            .Include(m => m.PhysicalCopies)
            .SingleAsync(m => m.Id == movie.Id);

        _logger.LogInformation("Movie {MovieId} created successfully", movie.Id);

        return Result.Ok(movie.MapToMovieDto());
    }
}