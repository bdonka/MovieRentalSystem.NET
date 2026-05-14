using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Domain.Entities;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(UserManager<User> userManager, ILogger<UpdateUserCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result> Handle(
        UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("User {UserId} updated", request.Id);

        var user = await _userManager.FindByIdAsync(request.Id);
        if (user == null)
        {
            _logger.LogWarning("User {UserId} not found", request.Id);
            return Result.Fail(new UserNotFoundError(request.Id));
        }

        var existingUser = await _userManager.FindByEmailAsync(request.Email);

        if (existingUser is not null && existingUser.Id != request.Id)
        {
            _logger.LogWarning("User with Email: {UserEmail} already exists", request.Email);
            return Result.Fail(new UserAlreadyExistsError(request.Email));
        }

        user.UserName = request.UserName;
        user.Email = request.Email;

        await _userManager.UpdateAsync(user);
        _logger.LogInformation("User {UserId} updated successfully", request.Id);
        return Result.Ok();
    }
}