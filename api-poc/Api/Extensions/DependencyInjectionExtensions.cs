using ChoresPoc.Api.Data;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;

namespace ChoresPoc.Api.Extensions;

public static class DependencyInjectionExtensions
{
	public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddFastEndpoints();
		services.AddSwagger();

		var connectionString = configuration.GetConnectionString("ChoreDbContext");
		services.AddDbContext<ChoreDbContext>(options =>
			options.UseSqlite(connectionString));
		
		return services;
	}

	private static IServiceCollection AddSwagger(this IServiceCollection services)
	{
		services.SwaggerDocument(options =>
		{
			options.EnableJWTBearerAuth = false;
		});
		
		return services;
	}
}