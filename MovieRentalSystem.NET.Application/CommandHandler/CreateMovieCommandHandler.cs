using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Domain.Entities;

public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, Result<MovieDto>>
{
    private readonly IDbContext _dbContext;

    public CreateMovieCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<MovieDto>> Handle(
        CreateMovieCommand request, CancellationToken cancellationToken)
    {
        if (await _dbContext.Movies.AnyAsync(m => m.Title == request.Title))
            return Result.Fail<MovieDto>($"Movie '{request.Title}' already exists.");

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
        return Result.Ok(movie.MapToMovieDto());
    }
}