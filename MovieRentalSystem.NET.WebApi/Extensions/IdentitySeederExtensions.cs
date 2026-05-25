using Microsoft.AspNetCore.Identity;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.WebApi.Common;

namespace MovieRentalSystem.NET.WebApi.Extensions;

public static class IdentitySeederExtensions
{
    public static async Task SeedIdentityAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await IdentitySeeder.SeedRolesAsync(roleManager);
        await IdentitySeeder.SeedUsersAsync(userManager);
    }
}
