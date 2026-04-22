using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Infrastructure.Data;
using MovieRentalSystem.NET.Infrastructure.Services;
using Microsoft.Extensions.Configuration;

namespace MovieRentalSystem.NET.Infrastructure.InfrastructureDependencies;

public static class InfrastructureDependencies
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("sqldata")));

        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IMoviePhysicalCopyService, MoviePhysicalCopyService>();
        services.AddScoped<IRentalService, RentalService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
