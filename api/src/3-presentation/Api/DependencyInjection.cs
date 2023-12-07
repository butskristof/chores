namespace Chores.Api;

internal static class DependencyInjection
{
    internal static IServiceCollection AddApi(this IServiceCollection services)
    {
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

        return services;
    }
}