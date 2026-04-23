using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Interfaces;

public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, Result>
{
    private readonly IDbContext _dbContext;

    public DeleteMovieCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(
        DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Id == request.Id);
        if (movie == null)
            return Result.Fail($"Movie with ID {request.Id} not found.");

        _dbContext.Movies.Remove(movie);
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
}