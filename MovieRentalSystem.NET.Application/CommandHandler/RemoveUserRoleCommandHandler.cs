using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Domain.Entities;
public class RemoveUserRoleCommandHandler : IRequestHandler<RemoveUserRoleCommand, Result>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<RemoveUserRoleCommandHandler> _logger;

    public RemoveUserRoleCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<RemoveUserRoleCommandHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }
    public async Task<Result> Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
    {

        _logger.LogInformation("Removing role {Role} from user {Id}", request.Role, request.Id);
        var user = await _userManager.FindByIdAsync(request.Id);

        if (user == null)
        {
            _logger.LogWarning("User {Id} not found", request.Id);
            return Result.Fail(new UserNotFoundError(request.Id));
        }

        var roleExists = await _roleManager.RoleExistsAsync(request.Role);

        if (!roleExists)
        {
            _logger.LogWarning("Role {Role} not found", request.Role);
            return Result.Fail(new RoleNotFoundError(request.Role));
        }

        var roles = await _userManager.GetRolesAsync(user);

        if (!roles.Contains(request.Role))
        {
            _logger.LogWarning("User {Id} does not have role {Role}", request.Id, request.Role);
            return Result.Fail(new UserNotHaveRoleError(request.Id, request.Role));
        }

        var result = await _userManager.RemoveFromRoleAsync(user, request.Role);

        if (!result.Succeeded)
        {
            _logger.LogWarning(
                "Failed to remove role {Role} from user {Id} because of following errors: {Errors}",
                request.Role,
                request.Id,
                result.Errors.Select(m => m.Description).ToArray());
            return Result.Fail(new UserNotRemoveRoleError(request.Id, request.Role));
        }

        _logger.LogInformation("Role {Role} successfully removed from user {Id}", request.Role, request.Id);
        return Result.Ok();
    }
}