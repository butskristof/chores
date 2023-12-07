using Chores.Application.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Chores.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services
            .AddDbContext<AppDbContext>(builder => builder
                .UseInMemoryDatabase("chores"));
        
        services
            .AddScoped<IAppDbContext, AppDbContext>();
        
        return services;
    }
}