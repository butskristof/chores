namespace Chores.Api.Modules;

internal static class TagsModule
{
    internal static IEndpointRouteBuilder MapTagsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/Tags")
            .WithTags("Tags");

        group.MapPost("", CreateTag)
            .WithName(nameof(CreateTag))
            .Produces(StatusCodes.Status204NoContent);
        group
            .MapGet("{id:guid}", (Guid id) => TypedResults.NotFound());

        return endpoints;
    }

    private static Task<IResult> CreateTag()
        => Task.FromResult(Results.NoContent());
}