using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Domain.Enums;

public class UpdateRentalCommandHandler : IRequestHandler<UpdateRentalCommand, Result>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<UpdateRentalCommandHandler> _logger;

    public UpdateRentalCommandHandler(IDbContext dbContext, ILogger<UpdateRentalCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(
        UpdateRentalCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating rental {RentalId} with new details", request.Id);

        var rental = await _dbContext.Rentals.FirstOrDefaultAsync(r => r.Id == request.Id);
        if (rental == null)
        {
            _logger.LogWarning("Rental {RentalId} not found", request.Id);
            return Result.Fail($"Rental {request.Id} not found.");
        }

        rental.RentalStartDate = request.RentalStartDate;
        rental.DueDate = request.DueDate;
        rental.ReturnDate = request.ReturnDate;
        rental.TotalPrice = request.TotalPrice;
        rental.Status = Enum.Parse<RentalStatus>(request.Status.ToString());

        if (request.ReturnDate != null)
        {
            _logger.LogInformation("Rental {RentalId} is being returned, updating copy status", request.Id); 
                var copy = await _dbContext.MoviePhysicalCopies
                .FirstOrDefaultAsync(c => c.Id == rental.MoviePhysicalCopyId);

            if (copy != null)
            {
                _logger.LogInformation("Updating movie copy {CopyId} status to Available", copy.Id);
                copy.Status = MovieCopyStatus.Available;
            }
        }

        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Rental {RentalId} updated successfully", request.Id);
        return Result.Ok();
    }
}