using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Interfaces;

public class UpdateMoviePhysicalCopyCommandHandler : IRequestHandler<UpdateMoviePhysicalCopyCommand, Result>
{
    private readonly IDbContext _dbContext;

    public UpdateMoviePhysicalCopyCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(
        UpdateMoviePhysicalCopyCommand request, CancellationToken cancellationToken)
    {
        var copy = await _dbContext.MoviePhysicalCopies.FirstOrDefaultAsync(c => c.Id == request.Id);
        if (copy == null)
            return Result.Fail($"Movie physical copy with Id {request.Id} not found.");

        copy.Status = request.Status;
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }
}