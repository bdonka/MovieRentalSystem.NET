using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieRentalSystem.NET.Application.Interfaces;
using MovieRentalSystem.NET.Infrastructure.Data;
using Microsoft.Extensions.Configuration;

namespace MovieRentalSystem.NET.Infrastructure.InfrastructureDependencies;

public static class InfrastructureDependencies
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("sqldata")));

        services.AddScoped<IDbContext, ApplicationDbContext>();
        return services;
    }
}
