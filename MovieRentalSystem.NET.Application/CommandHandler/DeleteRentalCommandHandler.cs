using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Interfaces;

public class DeleteRentalCommandHandler : IRequestHandler<DeleteRentalCommand, Result>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<DeleteRentalCommandHandler> _logger;

    public DeleteRentalCommandHandler(IDbContext dbContext, ILogger<DeleteRentalCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(
        DeleteRentalCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting rental {RentalId}", request.Id);

        var rental = await _dbContext.Rentals.FirstOrDefaultAsync(r => r.Id == request.Id);
        if (rental == null)
        {
            _logger.LogWarning("Rental {RentalId} not found", request.Id);
            return Result.Fail($"Rental {request.Id} not found.");
        }

        _dbContext.Rentals.Remove(rental);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Rental {RentalId} deleted successfully", request.Id);
        return Result.Ok();
    }
}