using System.Data.Common;
using Chores.Application.Common.Persistence;
using Chores.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

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

                Action<NpgsqlDbContextOptionsBuilder> npgsqlOptionsAction = optionsBuilder =>
                {
                    optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                };

                if (connectionString is not null)
                    builder.UseNpgsql(connectionString, npgsqlOptionsAction);
                else if (connection is not null)
                    builder.UseNpgsql(connection, npgsqlOptionsAction);
                else
                    throw new ArgumentException("Missing connection details to set up persistence");
            });

        services.AddScoped<ISaveChangesInterceptor, AuditingInterceptor>();
        services.AddScoped<IAppDbContext, AppDbContext>();

        return services;
    }
}