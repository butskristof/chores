namespace Chores.Api;

internal static class DependencyInjection
{
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
                options.CustomSchemaIds(type =>
                {
                    var innerClassNames = new[] { "Request", "Response", "Validator", "Handler" };
                    if (innerClassNames.Contains(type.Name))
                    {
                        // these types are probably contained in a static outer class
                        var outerClass = type.ReflectedType;
                        // if we find the outer class, return the schema ID as the type name 
                        // prefixed with the outer class name
                        if (outerClass != null) return $"{outerClass.Name}-{type.Name}";

                        // if we don't find the outer type name, fall back to the unique identifier
                        return type.GUID.ToString();
                    }

                    return type.Name;
                });
            });

        // add support for ProblemDetails to handle failed requests
        // it's effectively a default implementation of the IProblemDetailsService and can be
        // overridden if desired
        // it adds a default problem details response for all client and server error responses (400-599)
        // that don't have body content yet
        services.AddProblemDetails();

        return services;
    }
}