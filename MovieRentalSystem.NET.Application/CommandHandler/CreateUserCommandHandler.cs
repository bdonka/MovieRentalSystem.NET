using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Application.Dtos;
using MovieRentalSystem.NET.Application.Mappings;
using MovieRentalSystem.NET.Domain.Entities;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(UserManager<User> userManager, ILogger<CreateUserCommandHandler> logger)
    {
        _userManager = userManager;

        _logger = logger;
    }

    public async Task<Result<UserDto>> Handle(
        CreateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating user with email: {UserEmail}", request.Email);

        if (await _userManager.FindByEmailAsync(request.Email) is not null)
        {
            _logger.LogWarning("User already exists with email: {UserEmail}", request.Email);
            return Result.Fail<UserDto>(new UserAlreadyExistsError(request.Email));
        }

        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        _logger.LogInformation("User {UserId} created successfully", user.Id);
        return Result.Ok(user.MapToUserDto());
    }
}