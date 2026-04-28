using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;
    public GetUserByIdQueryHandler(IDbContext dbContext, ILogger<GetUserByIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<UserDto>> Handle(
        GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting user {UserId}", request.Id);
        var user = await _dbContext.Users.Include(u => u.Rentals).FirstOrDefaultAsync(u => u.Id == request.Id);
        if (user == null)
        {
            _logger.LogWarning("User {UserId} not found", request.Id);
            return Result.Fail<UserDto>($"User with ID {request.Id} not found.");
        }

        _logger.LogInformation("User {UserId} retrieved successfully", request.Id);
        return Result.Ok(user.MapToUserDto());
    }
}