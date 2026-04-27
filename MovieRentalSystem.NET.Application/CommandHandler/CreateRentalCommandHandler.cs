using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Domain.Entities;

public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, Result<RentalDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<CreateRentalCommandHandler> _logger;

    public CreateRentalCommandHandler(IDbContext dbContext, ILogger<CreateRentalCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<RentalDto>> Handle(
        CreateRentalCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating rental for User: {UserId} and MoviePhysicalCopy: {MoviePhysicalCopyId}", request.UserId, request.MoviePhysicalCopyId);
        if (!await _dbContext.Users.AnyAsync(u => u.Id == request.UserId))
        {
            _logger.LogWarning("User {UserId} does not exist.", request.UserId);
            return Result.Fail<RentalDto>($"User {request.UserId} does not exist.");
        }

        if (!await _dbContext.MoviePhysicalCopies.AnyAsync(c => c.Id == request.MoviePhysicalCopyId))
        {
            _logger.LogWarning("MoviePhysicalCopy {MoviePhysicalCopyId} does not exist.", request.MoviePhysicalCopyId);
            return Result.Fail<RentalDto>($"MoviePhysicalCopy {request.MoviePhysicalCopyId} does not exist.");
        }

        var rental = new Rental
        {
            UserId = request.UserId,
            MoviePhysicalCopyId = request.MoviePhysicalCopyId,
            RentalStartDate = request.RentalStartDate,
            DueDate = request.DueDate
        };

        _dbContext.Rentals.Add(rental);
        await _dbContext.SaveChangesAsync();

        rental = await _dbContext.Rentals
            .Include(r => r.User)
            .Include(r => r.MoviePhysicalCopy)
                .ThenInclude(m => m.Movie)
            .SingleAsync(r => r.Id == request.UserId);

        _logger.LogInformation("Rental {RentalId} created successfully", rental.Id);
        return Result.Ok(rental.MapToRentalDto());
    }
}