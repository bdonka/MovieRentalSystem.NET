using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Domain.Entities;

public class ReplaceUserRoleCommandHandler : IRequestHandler<ReplaceUserRoleCommand, Result>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ReplaceUserRoleCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result> Handle(
        ReplaceUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);
        if (user == null)
        {
            return Result.Fail(new UserNotFoundError(request.Id));
        }

        var roleExists = await _roleManager.RoleExistsAsync(request.Role);

        if (!roleExists)
            return Result.Fail(new RoleNotFoundError(request.Role));

        var currentRoles = await _userManager.GetRolesAsync(user);

        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

        if (!removeResult.Succeeded)
            return Result.Fail(new RoleNotRemoveFromUser(request.Id));

        var addResult = await _userManager.AddToRoleAsync(user, request.Role);

        if (!addResult.Succeeded)
            return Result.Fail(new NewRoleNotAssignToUser(request.Id, request.Role));

        return Result.Ok();
    }
}