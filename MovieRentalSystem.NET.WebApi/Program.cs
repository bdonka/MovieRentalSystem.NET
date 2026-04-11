using FluentValidation;
using Scalar.AspNetCore;
using System.Reflection.Metadata;
using MovieRentalSystem.NET.Infrastructure.InfrastructureDependencies;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddControllers();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(MovieRentalSystem.NET.Application.AssemblyReference).Assembly));

builder.Services.AddInfrastructureDependencies(builder.Configuration);

builder.Services.AddOpenApi();

builder.Services.AddValidatorsFromAssemblyContaining<AssemblyReference>();

var app = builder.Build();
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();

app.Run();