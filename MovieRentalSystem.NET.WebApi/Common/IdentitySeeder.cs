using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.Domain.Entities;

namespace MovieRentalSystem.NET.WebApi.Common;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "Admin", "Worker", "Customer" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    public static async Task SeedUsersAsync(
        UserManager<User> userManager)
    {
        if (await userManager.Users.AnyAsync())
            return;

        await CreateUser(userManager, "admin@email.com", "Admin123!", "Admin");
        await CreateUser(userManager, "worker@email.com", "Worker123!", "Worker");
        await CreateUser(userManager, "customer@email.com", "Customer123!", "Customer");
    }

    private static async Task CreateUser(
        UserManager<User> userManager,
        string email,
        string password,
        string role)
    {
        var user = new User
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}