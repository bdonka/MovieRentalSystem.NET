using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, Result<GenreDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<GetGenreByIdQueryHandler> _logger;
    public GetGenreByIdQueryHandler(IDbContext dbContext, ILogger<GetGenreByIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<GenreDto>> Handle(
        GetGenreByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting genre {GenreId}", request.Id);
        var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Id == request.Id);
        if (genre == null)
        {
            _logger.LogWarning("Genre {GenreId} not found", request.Id);
            return Result.Fail<GenreDto>($"Genre {request.Id} not found.");
        }

        _logger.LogInformation("Genre {GenreId} got successfully", request.Id);
        return Result.Ok(genre.MapToGenreDto());
    }
}