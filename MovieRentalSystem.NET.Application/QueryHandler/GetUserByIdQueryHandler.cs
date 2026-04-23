using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Application.Query;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IDbContext _dbContext;
    public GetUserByIdQueryHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<UserDto>> Handle(
        GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.Include(u => u.Rentals).FirstOrDefaultAsync(u => u.Id == request.Id);
        if (user == null)
            return Result.Fail<UserDto>($"User with ID {request.Id} not found.");
        return Result.Ok(user.MapToUserDto());
    }
}