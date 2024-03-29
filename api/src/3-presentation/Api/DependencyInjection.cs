using Chores.Api.Common;
using Chores.Application.Common.Authentication;
using Chores.Application.Common.Configuration;
using Chores.Application.Common.Constants;
using Chores.Persistence;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Chores.Api;

internal static class DependencyInjection
{
    internal static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .Configure<AuthenticationSettings>(configuration.GetSection(ConfigurationConstants.Authentication))
            .Configure<ClientSettings>(configuration.GetSection(ConfigurationConstants.Clients));

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

        services.AddCorsPolicy();

        services
            .AddHealthChecks()
            .AddDbContextCheck<AppDbContext>();

        return services;
    }
    
	private static IServiceCollection AddCorsPolicy(this IServiceCollection services)
	{
		using var serviceProvider = services.BuildServiceProvider();
		var configuration = serviceProvider.GetRequiredService<IOptions<ClientSettings>>().Value;

		services
			.AddCors(options => options
				.AddPolicy(ApplicationConstants.CorsPolicy,
					builder =>
					{
						builder.WithOrigins(configuration.ClientUrls);
						builder.AllowAnyMethod();
						builder.AllowAnyHeader();
					}));

		return services;
	}
}