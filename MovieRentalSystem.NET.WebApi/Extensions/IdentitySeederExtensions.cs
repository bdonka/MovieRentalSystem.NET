using Microsoft.AspNetCore.Identity;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.WebApi.Common;

namespace MovieRentalSystem.NET.WebApi.Extensions
{
    public static class IdentitySeederExtensions
    {
        public static async Task SeedRolesAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await IdentitySeeder.SeedRolesAsync(roleManager);
            }
        }

        public static async Task SeedAdminUserAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await IdentitySeeder.SeedAdminUserAsync(userManager, roleManager);
            }
        }

        public static async Task SeedWorkerUserAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await IdentitySeeder.SeedWorkerUserAsync(userManager, roleManager);
            }
        }

        public static async Task SeedCustomerUserAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await IdentitySeeder.SeedCustomerUserAsync(userManager, roleManager);
            }
        }
    }
}
