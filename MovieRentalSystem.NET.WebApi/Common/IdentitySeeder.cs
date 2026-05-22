using Microsoft.AspNetCore.Identity;
using MovieRentalSystem.NET.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MovieRentalSystem.NET.WebApi.Common;
public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "Admin", "Worker" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    public static async Task SeedAdminUserAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        string adminEmail = "admin@email.com";
        string adminPassword = "Admin123!";

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await userManager.Users.AnyAsync())
        {
            var adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            return;
        }

        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

        if (existingAdmin != null)
        {
            var roles = await userManager.GetRolesAsync(existingAdmin);

            if (!roles.Contains("Admin"))
            {
                await userManager.AddToRoleAsync(existingAdmin, "Admin");
            }
            return;
        }
    }

    public static async Task SeedWorkerUserAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        string workerEmail = "worker@email.com";
        string workerPassword = "Worker123!";

        if (!await roleManager.RoleExistsAsync("Worker"))
            await roleManager.CreateAsync(new IdentityRole("Worker"));

        var user = await userManager.FindByEmailAsync(workerEmail);

        if (user == null)
        {
            user = new User
            {
                UserName = workerEmail,
                Email = workerEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, workerPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Worker");
            }
        }
    }

    public static async Task SeedCustomerUserAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        string customerEmail = "customer@email.com";
        string password = "Customer123!";

        if (!await roleManager.RoleExistsAsync("Customer"))
            await roleManager.CreateAsync(new IdentityRole("Customer"));

        var user = await userManager.FindByEmailAsync(customerEmail);

        if (user == null)
        {
            user = new User
            {
                UserName = customerEmail,
                Email = customerEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Customer");
            }

            return;
        }

        var roles = await userManager.GetRolesAsync(user);

        var isAdminOrWorker =
            roles.Contains("Admin") ||
            roles.Contains("Worker");

        if (!isAdminOrWorker && !roles.Contains("Customer"))
        {
            await userManager.AddToRoleAsync(user, "Customer");
        }
    }
}