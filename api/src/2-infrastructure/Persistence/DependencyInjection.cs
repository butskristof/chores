using Chores.Application.Common.Constants;
using Chores.Application.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chores.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString(ConfigurationConstants.AppDbContextConnectionStringKey);
        services
            .AddDbContext<AppDbContext>(builder => builder
                .UseSqlServer(connectionString,
                    optionsBuilder =>
                    {
                        optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    }));

        services.AddScoped<IAppDbContext, AppDbContext>();

        return services;
    }
}