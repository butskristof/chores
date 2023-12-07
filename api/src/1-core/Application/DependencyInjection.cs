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
        // scan the current assembly for validators and register them with the DI container 
        // make sure to include internal types, since almost all of them should be defined as internal sealed class
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);

        services
            .AddMediatR(configuration =>
            {
                // scan the current assembly for requests and handlers and register them with the DI container
                // internals should be included by default, no additional configuration is necessary
                configuration
                    .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                // add cross-cutting concerns (supporting services) in the pipeline 
                // keep in mind that order of registration matters here
                configuration
                    .AddOpenBehavior(typeof(LoggingBehavior<,>))
                    .AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

        return services;
    }
}