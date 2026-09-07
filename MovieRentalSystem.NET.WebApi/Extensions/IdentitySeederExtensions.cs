using Microsoft.AspNetCore.Identity;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.WebApi.Extensions;

public static class IdentitySeederExtensions
{
    public static async Task SeedIdentityAsync(
        this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var roleManager =
            scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

        var userManager =
            scope.ServiceProvider
                .GetRequiredService<UserManager<User>>();

        var roles = new[]
        {
            "Admin",
            "Worker",
            "Customer"
        };

        foreach (var role in roles)
        {
            await EnsureRoleAsync(roleManager, role);
        }

        // Admin
        var adminEmail = "admin@test.com";

        var admin =
            await userManager.FindByEmailAsync(adminEmail);

        if (admin is null)
        {
            admin = new User
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result =
                await userManager.CreateAsync(
                    admin,
                    "Admin123!");

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(
                        ", ",
                        result.Errors.Select(
                            e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(admin, "Admin"))
        {
            var roleResult =
                await userManager.AddToRoleAsync(
                    admin,
                    "Admin");

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(
                        ", ",
                        roleResult.Errors.Select(
                            e => e.Description)));
            }
        }
    }

    private static async Task EnsureRoleAsync(
        RoleManager<IdentityRole> roleManager,
        string roleName)
    {
        if (await roleManager.RoleExistsAsync(roleName))
        {
            return;
        }

        var result =
            await roleManager.CreateAsync(
                new IdentityRole(roleName));

        if (result.Succeeded)
        {
            return;
        }

        if (await roleManager.RoleExistsAsync(roleName))
        {
            return;
        }

        throw new InvalidOperationException(
            $"Failed to create Identity role '{roleName}': " +
            string.Join(
                ", ",
                result.Errors.Select(
                    e => e.Description)));
    }
}
