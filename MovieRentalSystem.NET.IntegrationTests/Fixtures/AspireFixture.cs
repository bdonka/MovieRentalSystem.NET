using System.Net;
using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Infrastructure.Data;
using MovieRentalSystem.NET.IntegrationTests.Helpers;
using Projects;

namespace MovieRentalSystem.NET.IntegrationTests.Fixtures;

public class AspireFixture : IAsyncLifetime
{
    private static readonly TimeSpan StartupTimeout =
        TimeSpan.FromSeconds(120);

    private DistributedApplication _app = null!;
    private string _connectionString = null!;

    public async Task InitializeAsync()
    {
        var builder =
            await DistributedApplicationTestingBuilder
                .CreateAsync<MovieRentalSystem_NET_AppHost>();

        _app = await builder.BuildAsync();

        await _app.StartAsync();

        await _app.ResourceNotifications
            .WaitForResourceHealthyAsync("api")
            .WaitAsync(StartupTimeout);

        _connectionString =
            await _app.GetConnectionStringAsync("sqldata")
            ?? throw new InvalidOperationException(
                "Connection string 'sqldata' was not found.");
    }

    public HttpClient CreateClient()
    {
        var aspireClient =
            _app.CreateHttpClient("api");

        var baseAddress =
            aspireClient.BaseAddress
            ?? throw new InvalidOperationException(
                "API client does not have a BaseAddress.");

        aspireClient.Dispose();

        var cookieContainer =
            new CookieContainer();

        var handler =
            new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true,
                AllowAutoRedirect = false
            };

        return new HttpClient(handler)
        {
            BaseAddress = baseAddress
        };
    }

    public AuthTestHelper CreateAuthHelper(
        HttpClient client)
    {
        return new AuthTestHelper(client);
    }

    public async Task<TestUserData> CreateAdminAsync()
    {
        var userData = TestUserData.Admin();

        await using var serviceProvider =
            BuildIdentityServiceProvider();

        using var scope =
            serviceProvider.CreateScope();

        var userManager =
            scope.ServiceProvider
                .GetRequiredService<UserManager<User>>();

        var user = new User
        {
            UserName = userData.UserName,
            Email = userData.Email,
            EmailConfirmed = true
        };

        var createUserResult =
            await userManager.CreateAsync(
                user,
                userData.Password);

        if (!createUserResult.Succeeded)
        {
            throw new InvalidOperationException(
                "Could not create test admin user: " +
                string.Join(
                    ", ",
                    createUserResult.Errors
                        .Select(error => error.Description)));
        }

        var addRoleResult =
            await userManager.AddToRoleAsync(
                user,
                "Admin");

        if (!addRoleResult.Succeeded)
        {
            throw new InvalidOperationException(
                "Could not add test admin user to Admin role: " +
                string.Join(
                    ", ",
                    addRoleResult.Errors
                        .Select(error => error.Description)));
        }

        var roles =
            await userManager.GetRolesAsync(user);

        if (!roles.Contains("Admin"))
        {
            throw new InvalidOperationException(
                "Test admin user was created but does not have the Admin role.");
        }

        return userData;
    }

    public async Task<TestUserData> CreateWorkerAsync()
    {
        var userData = TestUserData.Worker();

        await using var serviceProvider =
            BuildIdentityServiceProvider();

        using var scope =
            serviceProvider.CreateScope();

        var userManager =
            scope.ServiceProvider
                .GetRequiredService<UserManager<User>>();

        var user = new User
        {
            UserName = userData.UserName,
            Email = userData.Email,
            EmailConfirmed = true
        };

        var createUserResult =
            await userManager.CreateAsync(
                user,
                userData.Password);

        if (!createUserResult.Succeeded)
        {
            throw new InvalidOperationException(
                "Could not create test worker user: " +
                string.Join(
                    ", ",
                    createUserResult.Errors
                        .Select(error => error.Description)));
        }

        var addRoleResult =
            await userManager.AddToRoleAsync(
                user,
                "Worker");

        if (!addRoleResult.Succeeded)
        {
            throw new InvalidOperationException(
                "Could not add test worker user to Worker role: " +
                string.Join(
                    ", ",
                    addRoleResult.Errors
                        .Select(error => error.Description)));
        }

        var roles =
            await userManager.GetRolesAsync(user);

        if (!roles.Contains("Worker"))
        {
            throw new InvalidOperationException(
                "Test worker user was created but does not have the Worker role.");
        }

        return userData;
    }

    public async Task DeleteUserAsync(
        TestUserData userData)
    {
        await using var serviceProvider =
            BuildIdentityServiceProvider();

        using var scope =
            serviceProvider.CreateScope();

        var userManager =
            scope.ServiceProvider
                .GetRequiredService<UserManager<User>>();

        var user =
            await userManager.FindByEmailAsync(
                userData.Email);

        if (user is null)
        {
            return;
        }

        var result =
            await userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                "Could not delete test user: " +
                string.Join(
                    ", ",
                    result.Errors
                        .Select(error => error.Description)));
        }
    }

    private ServiceProvider BuildIdentityServiceProvider()
    {
        var services =
            new ServiceCollection();

        services.AddLogging();

        services.AddDbContext<ApplicationDbContext>(
            options =>
                options.UseSqlServer(_connectionString));

        services
            .AddIdentity<User, IdentityRole>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.User.RequireUniqueEmail = false;
                })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services.BuildServiceProvider();
    }

    public async Task DisposeAsync()
    {
        await _app.DisposeAsync();
    }
}
