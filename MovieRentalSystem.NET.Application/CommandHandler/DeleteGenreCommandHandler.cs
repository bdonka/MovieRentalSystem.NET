using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Interfaces;

public class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommand, Result>
{
    private readonly IDbContext _dbContext;

    public DeleteGenreCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(
        DeleteGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Id == request.Id);
        if (genre == null)
            return Result.Fail($"Genre with ID {request.Id} not found.");
        _dbContext.Genres.Remove(genre);
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
}