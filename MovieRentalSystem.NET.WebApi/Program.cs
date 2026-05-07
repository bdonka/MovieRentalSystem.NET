using FluentValidation;
using Microsoft.AspNetCore.HttpLogging;
using MovieRentalSystem.NET.Application.ApplicationDependencies;
using MovieRentalSystem.NET.Infrastructure.InfrastructureDependencies;
using MovieRentalSystem.NET.WebApi.Middlewares;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(new Serilog.Formatting.Compact.RenderedCompactJsonFormatter())
    .WriteTo.OpenTelemetry()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

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

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseHttpLogging();

app.UseMiddleware<ResponseTimeMiddleware>();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();

app.Run();