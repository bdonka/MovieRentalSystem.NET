using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, Result<MovieDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<GetMovieByIdQueryHandler> _logger;
    public GetMovieByIdQueryHandler(IDbContext dbContext, ILogger<GetMovieByIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<MovieDto>> Handle(
        GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting movie {MovieId}", request.Id);

        var movie = await _dbContext.Movies
            .Include(m => m.Genres)
            .Include(m => m.PhysicalCopies)
            .FirstOrDefaultAsync(m => m.Id == request.Id);
        if (movie == null)
        {
            _logger.LogWarning("Movie {MovieId} not found", request.Id);
            return Result.Fail<MovieDto>($"Movie {request.Id} not found.");
        }

        _logger.LogInformation("Movie {MovieId} retrieved successfully", request.Id);
        return Result.Ok(movie.MapToMovieDto());
    }
}