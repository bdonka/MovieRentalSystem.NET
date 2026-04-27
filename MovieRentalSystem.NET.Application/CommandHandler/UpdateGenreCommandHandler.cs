using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Interfaces;

public class UpdateGenreCommandHandler : IRequestHandler<UpdateGenreCommand, Result>
{
    private readonly IDbContext _dbContext;

    public UpdateGenreCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(
        UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Id == request.Id);
        if (genre == null)
            return Result.Fail($"Genre with ID {request.Id} not found.");

        if (await _dbContext.Genres.AnyAsync(g => g.Name == request.Name && g.Id != request.Id))
            return Result.Fail($"Genre '{request.Name}' already exists.");
        genre.Name = request.Name;
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
}