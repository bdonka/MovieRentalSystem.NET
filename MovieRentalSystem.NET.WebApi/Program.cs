using FluentValidation;
using Microsoft.AspNetCore.HttpLogging;
using MovieRentalSystem.NET.Application.ApplicationDependencies;
using MovieRentalSystem.NET.Infrastructure.InfrastructureDependencies;
using MovieRentalSystem.NET.WebApi.Middlewares;
using Scalar.AspNetCore;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.Request | HttpLoggingFields.Response;
});

//Log.Logger = new LoggerConfiguration()
//    .ReadFrom.Configuration(builder.Configuration)
//    .WriteTo.Console()
//    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
//    .CreateLogger();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

builder.AddServiceDefaults();

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddApplicationDependencies();

builder.Services.AddInfrastructureDependencies(builder.Configuration);

builder.Services.AddOpenApi();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

builder.Services.AddScoped<ResponseTimeMiddleware>();

var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate =
        "HTTP {Method} {Path} => {StatusCode} in {Elapsed} ms";
});

app.UseExceptionHandler();

app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.UseMiddleware<ResponseTimeMiddleware>();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();

app.MapGet("/", () => "Hello World");

app.Run();