using System.Reflection;
using Chores.Application.Common.MediatR.PipelineBehaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Chores.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // FluentValidation
        services
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);

        // MediatR
        services
            .AddMediatR(configuration =>
            {
                configuration
                    .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                configuration
                    .AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

        return services;
    }
}