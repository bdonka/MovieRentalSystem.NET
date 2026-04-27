using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Domain.Entities;
public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, Result<GenreDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<CreateGenreCommandHandler> _logger;

    public CreateGenreCommandHandler(IDbContext dbContext, ILogger<CreateGenreCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<GenreDto>> Handle(
        CreateGenreCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating genre {GenreName}", request.Name);

        if (await _dbContext.Genres.AnyAsync(g => g.Name == request.Name))
        {
            _logger.LogWarning("Genre already exists {GenreName}", request.Name);
            return Result.Fail<GenreDto>($"Genre '{request.Name}' already exists.");
        }


        var genre = new Genre
        {
            Name = request.Name
        };

        _dbContext.Genres.Add(genre);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Genre created successfully {GenreId}", genre.Id);

        return Result.Ok(genre.MapToGenreDto());
    }
}