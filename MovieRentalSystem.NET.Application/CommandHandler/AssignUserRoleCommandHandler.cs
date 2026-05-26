using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Domain.Entities;
public class AssignUserRoleCommandHandler : IRequestHandler<AssignUserRoleCommand, Result>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AssignUserRoleCommandHandler> _logger;

    public AssignUserRoleCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<AssignUserRoleCommandHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task<Result> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Assigning role {Role} to user {Id}", request.Id, request.Role);
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

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Contains(request.Role))
        {
            _logger.LogWarning("User {Id} already has role", request.Id);
            return Result.Fail(new UserAlreadyHasRoleError(request.Id));
        }

        var result = await _userManager.AddToRoleAsync(user, request.Role);

        if (!result.Succeeded)
        {
            _logger.LogWarning(
                "Role {Role} not assign to user {Id} because of following errors: {Errors}", 
                request.Role, 
                request.Id, 
                result.Errors.Select(m => m.Description).ToArray());
            return Result.Fail(new RoleNotAssignToUserError(request.Role, request.Id));
        }

        _logger.LogInformation("Role {Role} assigned to user {Id}", request.Role, request.Id);
        return Result.Ok();
    }
}