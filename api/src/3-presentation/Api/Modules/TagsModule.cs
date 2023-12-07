using Chores.Application.Modules.Tags;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chores.Api.Modules;

internal static class TagsModule
{
    internal static IEndpointRouteBuilder MapTagsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/Tags")
            .WithTags("Tags");

        group
            .MapPost("", CreateTag)
            .WithName(nameof(CreateTag))
            .Produces(StatusCodes.Status201Created, typeof(CreateTag.Response));
        group.MapGet("{id:guid}", (Guid id) => TypedResults.NotFound());

        return endpoints;
    }

    private static async Task<IResult> CreateTag([FromBody] CreateTag.Request request, ISender sender)
    {
        var result = await sender.Send(request);
        return TypedResults.Created($"/Tags/{result.Id}", result);
    }
}