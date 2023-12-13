using System.Data.Common;
using Chores.Application.Common.Persistence;
using Chores.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Chores.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, string? connectionString = null,
        DbConnection? connection = null)
    {
        services
            .AddDbContext<AppDbContext>((serviceProvider, builder) =>
            {
                builder.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());

                Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = optionsBuilder =>
                {
                    optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                };

                if (connectionString is not null)
                    builder.UseSqlServer(connectionString, sqlServerOptionsAction);
                else if (connection is not null)
                    builder.UseSqlServer(connection, sqlServerOptionsAction);
                else
                    throw new ArgumentException("Missing connection details to set up persistence");
            });

        services.AddScoped<ISaveChangesInterceptor, AuditingInterceptor>();
        services.AddScoped<IAppDbContext, AppDbContext>();

        return services;
    }
}