using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Interfaces;

public class UpdateMoviePhysicalCopyCommandHandler : IRequestHandler<UpdateMoviePhysicalCopyCommand, Result>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<UpdateMoviePhysicalCopyCommandHandler> _logger;

    public UpdateMoviePhysicalCopyCommandHandler(IDbContext dbContext, ILogger<UpdateMoviePhysicalCopyCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(
        UpdateMoviePhysicalCopyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating movie physical copy {CopyId} to status: {Status}", request.Id, request.Status);

        var copy = await _dbContext.MoviePhysicalCopies.FirstOrDefaultAsync(c => c.Id == request.Id);
        if (copy == null)
        {
            _logger.LogWarning("Movie physical copy{CopyId} not found", request.Id);
            return Result.Fail($"Movie physical copy {request.Id} not found.");
        }

        copy.Status = request.Status;
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Movie physical copy {CopyId} updated successfully", request.Id);
        return Result.Ok();
    }
}