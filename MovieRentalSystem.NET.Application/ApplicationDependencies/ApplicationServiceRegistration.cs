using Microsoft.Extensions.DependencyInjection;

namespace MovieRentalSystem.NET.Application.ApplicationDependency;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        return services;
    }
}
