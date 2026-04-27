using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Domain.Entities;
public class CreateGenreCommandHandler : IRequestHandler<CreateGenreCommand, Result<GenreDto>>
{
    private readonly IDbContext _dbContext;

    public CreateGenreCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<GenreDto>> Handle(
        CreateGenreCommand request, CancellationToken cancellationToken)
    {
        if (await _dbContext.Genres.AnyAsync(g => g.Name == request.Name))
            return Result.Fail<GenreDto>($"Genre '{request.Name}' already exists.");

        var genre = new Genre
        {
            Name = request.Name
        };
        _dbContext.Genres.Add(genre);
        await _dbContext.SaveChangesAsync();

        return Result.Ok(genre.MapToGenreDto());
    }
}