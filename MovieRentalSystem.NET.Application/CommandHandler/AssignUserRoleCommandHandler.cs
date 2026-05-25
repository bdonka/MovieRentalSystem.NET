using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MovieRentalSystem.NET.Application.Common;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Domain.Entities;
public class AssignUserRoleCommandHandler : IRequestHandler<AssignUserRoleCommand, Result>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AssignUserRoleCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);

        if (user == null)
            return Result.Fail(new UserNotFoundError(request.Id));

        var roleExists = await _roleManager.RoleExistsAsync(request.Role);

        if (!roleExists)
            return Result.Fail(new RoleNotFoundError(request.Role));

        var currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Contains(request.Role))
            return Result.Fail(new UserAlreadyHasRoleError(request.Id));

        var result = await _userManager.AddToRoleAsync(user, request.Role);

        if (!result.Succeeded)
            return Result.Fail(new RoleNotAssignToUserError(request.Role, request.Id));

        return Result.Ok();
    }
}