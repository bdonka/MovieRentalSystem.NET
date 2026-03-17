using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MovieRentalSystem.NET.WebApi.Data;
using MovieRentalSystem.NET.WebApi.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Add services to the container
builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<MovieService, MovieService>();
builder.Services.AddScoped<GenreService, GenreService>();
builder.Services.AddScoped<MovieGenreService, MovieGenreService>();
builder.Services.AddScoped<MoviePhysicalCopyService, MoviePhysicalCopyService>();
builder.Services.AddScoped<RentalService, RentalService>();
builder.Services.AddScoped<UserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapControllers();

app.Run();