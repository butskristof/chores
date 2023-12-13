namespace Chores.Api.Extensions;

internal static class RouteBuilderExtensions
{
    // these extensions are shorthand for adding OpenAPI specifications to minimal API endpoints

    internal static RouteHandlerBuilder ProducesOk<T>(this RouteHandlerBuilder builder)
        => builder.Produces(StatusCodes.Status200OK, typeof(T));

    internal static RouteHandlerBuilder ProducesCreated<T>(this RouteHandlerBuilder builder)
        => builder.Produces(StatusCodes.Status201Created, typeof(T));

    internal static RouteHandlerBuilder ProducesNoContent(this RouteHandlerBuilder builder)
        => builder.Produces(StatusCodes.Status204NoContent);

    internal static RouteHandlerBuilder ProducesForbidden(this RouteHandlerBuilder builder)
        => builder.ProducesProblem(StatusCodes.Status403Forbidden);

    internal static RouteHandlerBuilder ProducesNotFound(this RouteHandlerBuilder builder)
        => builder.ProducesProblem(StatusCodes.Status404NotFound);

    internal static RouteHandlerBuilder ProducesConflict(this RouteHandlerBuilder builder)
        => builder.ProducesProblem(StatusCodes.Status409Conflict);
}