using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Interfaces;

public class DeleteMoviePhysicalCopyCommandHandler : IRequestHandler<DeleteMoviePhysicalCopyCommand, Result>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<DeleteMoviePhysicalCopyCommandHandler> _logger;

    public DeleteMoviePhysicalCopyCommandHandler(IDbContext dbContext, ILogger<DeleteMoviePhysicalCopyCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeleteMoviePhysicalCopyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting movie physical copy {CopyId}", request.Id);

        var copy = await _dbContext.MoviePhysicalCopies.FirstOrDefaultAsync(c => c.Id == request.Id);
        if (copy == null)
        {
            _logger.LogWarning("Movie physical copy {CopyId} not found", request.Id);
            return Result.Fail($"Movie physical copy {request.Id} not found.");
        }

        _dbContext.MoviePhysicalCopies.Remove(copy);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Movie physical copy {CopyId} deleted successfully", request.Id);
        return Result.Ok();
    }
}