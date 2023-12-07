using Microsoft.AspNetCore.Mvc;

namespace Chores.Api.Extensions;

internal static class RouteBuilderExtensions
{
    // these extensions are shorthand for adding OpenAPI specifications to minimal API endpoints

    internal static RouteHandlerBuilder ProducesCreated<T>(this RouteHandlerBuilder builder)
        => builder.Produces(StatusCodes.Status201Created, typeof(T));

    internal static RouteHandlerBuilder ProducesNoContent(this RouteHandlerBuilder builder)
        => builder.Produces(StatusCodes.Status204NoContent);

    internal static RouteHandlerBuilder ProducesValidationProblem(this RouteHandlerBuilder builder)
        => builder.Produces(StatusCodes.Status400BadRequest, typeof(ValidationProblemDetails));

    internal static RouteHandlerBuilder ProducesForbidden(this RouteHandlerBuilder builder)
        => builder.Produces(StatusCodes.Status403Forbidden, typeof(ProblemDetails));

    internal static RouteHandlerBuilder ProducesNotFound(this RouteHandlerBuilder builder)
        => builder.Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails));

    internal static RouteHandlerBuilder ProducesConflict(this RouteHandlerBuilder builder)
        => builder.Produces(StatusCodes.Status409Conflict, typeof(ProblemDetails));
}