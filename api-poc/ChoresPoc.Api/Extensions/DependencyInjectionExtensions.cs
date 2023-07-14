namespace ChoresPoc.Api.Extensions;

public static class DependencyInjectionExtensions
{
	public static IServiceCollection AddApi(this IServiceCollection services)
	{
		services.AddSwagger();
		
		return services;
	}

	private static IServiceCollection AddSwagger(this IServiceCollection services)
	{
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();
		
		return services;
	}
}