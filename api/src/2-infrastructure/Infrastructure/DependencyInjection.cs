using Microsoft.Extensions.DependencyInjection;

namespace Chores.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // the default implementation of TimeProvider can be registered as a singleton
        services.AddSingleton(TimeProvider.System);

        return services;
    }
}