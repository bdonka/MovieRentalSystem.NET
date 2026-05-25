using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MovieRentalSystem.NET.Application.Common.Errors;
using MovieRentalSystem.NET.Domain.Entities;
public class RemoveUserRoleCommandHandler : IRequestHandler<RemoveUserRoleCommand, Result>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RemoveUserRoleCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public async Task<Result> Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);

        if (user == null)
        {
            return Result.Fail(new UserNotFoundError(request.Id));
        }

        var roleExists = await _roleManager.RoleExistsAsync(request.Role);

        if (!roleExists)
            return Result.Fail(new RoleNotFoundError(request.Role));

        var roles = await _userManager.GetRolesAsync(user);

        if (!roles.Contains(request.Role))
            return Result.Fail(new UserNotHaveRoleError(request.Id, request.Role));

        var result = await _userManager.RemoveFromRoleAsync(user, request.Role);

        if (!result.Succeeded)
            return Result.Fail(new UserNotRemoveRoleError(request.Id, request.Role));

        return Result.Ok();
    }
}