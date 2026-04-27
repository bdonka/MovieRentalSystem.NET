using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetRentalByIdQueryHandler : IRequestHandler<GetRentalByIdQuery, Result<RentalDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<GetRentalByIdQueryHandler> _logger;
    public GetRentalByIdQueryHandler(IDbContext dbContext, ILogger<GetRentalByIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<RentalDto>> Handle(
        GetRentalByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting rental {RentalId}", request.Id);

        var rental = await _dbContext.Rentals
                    .Include(r => r.User)
                    .Include(r => r.MoviePhysicalCopy)
                        .ThenInclude(m => m.Movie)
                    .FirstOrDefaultAsync(r => r.Id == request.Id);
        if (rental == null)
        {
            _logger.LogWarning("Rental {RentalId} not found", request.Id);
            return Result.Fail($"Rental {request.Id} not found.");
        }

        _logger.LogInformation("Rental {RentalId} retrieved successfully", request.Id);
        return Result.Ok(rental.MapToRentalDto());
    }
}