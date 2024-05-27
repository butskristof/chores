using Chores.Api.Common;
using Chores.Application.Common.Authentication;
using Chores.Application.Common.Configuration;
using Chores.Application.Common.Constants;
using Chores.Application.Common.FluentValidation;
using Chores.Persistence;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Chores.Api;

internal static class DependencyInjection
{
    internal static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        services
            .AddValidatedSettings<AuthenticationSettings>(ConfigurationConstants.Authentication);

        return services;
    }

    private static IServiceCollection AddValidatedSettings<TOptions>(this IServiceCollection services,
        string sectionName)
        where TOptions : class
    {
        services
            .AddOptions<TOptions>()
            .BindConfiguration(sectionName);
            // .FluentValidateOptions()
            // .ValidateOnStart();

        return services;
    }

    internal static IServiceCollection AddApi(this IServiceCollection services)
    {
        // set up Swagger to generate OpenAPI definitions
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                // by default, Swagger will try to use the type name as schema ID 
                // since we're using static classes with inner Request, Response, ... types
                // these would be flagged as conflicting definitions
                options.CustomSchemaIds(type => type.FullName?.Replace('+', '.'));
            });

        // add support for ProblemDetails to handle failed requests
        // it's effectively a default implementation of the IProblemDetailsService and can be
        // overridden if desired
        // it adds a default problem details response for all client and server error responses (400-599)
        // that don't have body content yet
        services.AddProblemDetails();

        services
            .AddHttpContextAccessor()
            .AddScoped<IAuthenticationInfo, ApiAuthenticationInfo>();

        services
            .AddAuthentication()
            .AddJwtBearer(options =>
            {
                using var serviceProvider = services.BuildServiceProvider();
                var configuration = serviceProvider
                    .GetRequiredService<IOptions<AuthenticationSettings>>()
                    .Value;

                options.Authority = configuration.Issuer;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudiences = configuration.Audiences,
                    ValidateIssuer = true,
                    ValidIssuer = configuration.Issuer,
                    ValidateIssuerSigningKey = true,
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                };
            });
        services.AddAuthorization();

        services
            .AddHealthChecks()
            .AddDbContextCheck<AppDbContext>();

        return services;
    }

    internal static LoggerConfiguration WriteToConsole(this LoggerConfiguration loggerConfiguration)
    {
        return loggerConfiguration
            .Enrich.FromLogContext()
            .WriteTo.Console(
                theme: AnsiConsoleTheme.Code,
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} {NewLine}{Message:lj} {NewLine}{Exception}"
            );
    }
}