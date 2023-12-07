using Microsoft.Extensions.DependencyInjection;

namespace Chores.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        
        return services;
    }
}