using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;

public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, Result>
{
    private readonly IDbContext _dbContext;

    public UpdateMovieCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(
        UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Id == request.Id);
        if (movie == null)
            return Result.Fail($"Movie with ID {request.Id} not found.");

        if (await _dbContext.Movies.AnyAsync(m => m.Title == request.Title && m.Id != request.Id    ))
            return Result.Fail($"Movie '{request.Title}' already exists.");

        movie.Title = request.Title;
        movie.Description = request.Description;
        movie.ReleaseYear = request.ReleaseYear;
        movie.RentalPrice = request.RentalPrice;

        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
}