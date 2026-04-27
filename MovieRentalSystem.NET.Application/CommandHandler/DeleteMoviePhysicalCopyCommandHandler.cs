using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Interfaces;

public class DeleteMoviePhysicalCopyCommandHandler : IRequestHandler<DeleteMoviePhysicalCopyCommand, Result>
{
    private readonly IDbContext _dbContext;

    public DeleteMoviePhysicalCopyCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(
        DeleteMoviePhysicalCopyCommand request, CancellationToken cancellationToken)
    {
        var copy = await _dbContext.MoviePhysicalCopies.FirstOrDefaultAsync(c => c.Id == request.Id);
        if (copy == null)
            return Result.Fail($"Movie physical copy with Id {request.Id} not found.");

        _dbContext.MoviePhysicalCopies.Remove(copy);
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
}