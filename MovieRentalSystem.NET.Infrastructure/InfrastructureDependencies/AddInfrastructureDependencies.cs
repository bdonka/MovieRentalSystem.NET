using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Infrastructure.Data;
using MovieRentalSystem.NET.Infrastructure.Services;

namespace MovieRentalSystem.NET.Infrastructure.InfrastructureDependencies;

public static class AddInfrastructureDependencies
{
    public static IServiceCollection InfrastructureDependencies(this IServiceCollection services)
    {

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString: "sqldata"));

        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IMoviePhysicalCopyService, MoviePhysicalCopyService>();
        services.AddScoped<IRentalService, RentalService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
