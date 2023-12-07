using Chores.Api.Middlewares;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

namespace Chores.Api;

internal static class DependencyInjection
{
    internal static IServiceCollection AddApi(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type =>
                {
                    var outerClass = type.ReflectedType;
                    if (outerClass != null) return $"{outerClass.Name}-{type.Name}";
                    if (type.Name != "Request" && type.Name != "Response") return type.Name;
                    return type.GUID.ToString();
                });
            })
            .AddFluentValidationRulesToSwagger();

        services
            .AddProblemDetails()
            .AddExceptionHandler<ExceptionToProblemDetailsHandler>();

        return services;
    }
}