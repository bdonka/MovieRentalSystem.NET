using Microsoft.AspNetCore.Identity;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.WebApi.Common;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(
        RoleManager<IdentityRole> roleManager)
    {
        string[] roles =
        {
            "Admin",
            "Worker",
            "Customer"
        };

        foreach (var role in roles)
        {
            if (await roleManager.RoleExistsAsync(role))
            {
                continue;
            }

            var result =
                await roleManager.CreateAsync(
                    new IdentityRole(role));

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(
                        ", ",
                        result.Errors.Select(
                            x => x.Description)));
            }
        }
    }

    public static async Task SeedUsersAsync(
        UserManager<User> userManager)
    {
        await CreateUser(
            userManager,
            "admin@test.com",
            "Admin123!",
            "Admin");

        await CreateUser(
            userManager,
            "worker@test.com",
            "Worker123!",
            "Worker");

        await CreateUser(
            userManager,
            "customer@test.com",
            "Customer123!",
            "Customer");
    }

    private static async Task CreateUser(
        UserManager<User> userManager,
        string email,
        string password,
        string role)
    {
        var user =
            await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var createResult =
                await userManager.CreateAsync(
                    user,
                    password);

            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(
                        ", ",
                        createResult.Errors.Select(
                            x => x.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(
                user,
                role))
        {
            var roleResult =
                await userManager.AddToRoleAsync(
                    user,
                    role);

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(
                        ", ",
                        roleResult.Errors.Select(
                            x => x.Description)));
            }
        }
    }
}
