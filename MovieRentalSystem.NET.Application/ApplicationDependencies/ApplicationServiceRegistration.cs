using Microsoft.Extensions.DependencyInjection;

namespace MovieRentalSystem.NET.Application.ApplicationDependencies;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly)); 
        return services;
    }
}
