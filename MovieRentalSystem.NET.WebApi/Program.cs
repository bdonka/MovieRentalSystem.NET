using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using MovieRentalSystem.NET.Application.ApplicationDependencies;
using MovieRentalSystem.NET.Domain.Entities;
using MovieRentalSystem.NET.Infrastructure.Data;
using MovieRentalSystem.NET.Infrastructure.InfrastructureDependencies;
using MovieRentalSystem.NET.WebApi.Extensions;
using MovieRentalSystem.NET.WebApi.Middlewares;
using MovieRentalSystem.NET.WebApi.Validators.Genres;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = false;
})
.AddRoles<IdentityRole>()
.AddSignInManager<SignInManager<User>>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = null;
    options.AccessDeniedPath = null;

    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

    options.ExpireTimeSpan =
        TimeSpan.FromMinutes(60);

    options.SlidingExpiration = true;

    options.Events.OnValidatePrincipal = context =>
    {
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode =
            StatusCodes.Status401Unauthorized;

        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode =
            StatusCodes.Status403Forbidden;

        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization();

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields =
        HttpLoggingFields.RequestPath |
        HttpLoggingFields.RequestHeaders |
        HttpLoggingFields.RequestBody |
        HttpLoggingFields.ResponseStatusCode |
        HttpLoggingFields.ResponseHeaders |
        HttpLoggingFields.ResponseBody;

    options.RequestBodyLogLimit = 4096;
    options.ResponseBodyLogLimit = 4096;
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override(
        "Microsoft",
        LogEventLevel.Information)
    .MinimumLevel.Override(
        "Microsoft.AspNetCore",
        LogEventLevel.Warning)
    .MinimumLevel.Override(
        "Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware",
        LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.OpenTelemetry()
    .WriteTo.File(
        "logs/log.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddSerilog();

builder.AddServiceDefaults();

builder.Services
    .AddFluentValidationAutoValidation();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddApplicationDependencies();

builder.Services.AddInfrastructureDependencies(
    builder.Configuration);

builder.Services.AddOpenApi();

builder.Services.AddValidatorsFromAssemblyContaining<
    CreateGenreRequestValidator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

builder.Services.AddScoped<ResponseTimeMiddleware>();

var app = builder.Build();

await app.SeedIdentityAsync();

app.UseExceptionHandler();

app.UseHttpLogging();

app.UseMiddleware<ResponseTimeMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();
    app.MapScalarApiReference();
}

app.MapControllers();

app.Run();
