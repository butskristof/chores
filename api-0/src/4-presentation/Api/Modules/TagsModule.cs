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
            .MapGet("", GetTags)
            .WithName(nameof(GetTags));
        group
            .MapGet("{id:guid}", (Guid id) => TypedResults.NotFound());
        group
            .MapPost("", CreateTag)
            .WithName(nameof(CreateTag));

        return endpoints;
    }

    private static Task<GetTags.Response> GetTags(ISender sender)
        => sender.Send(new GetTags.Request());

    private static async Task<IResult> CreateTag([FromBody] CreateTag.Request request,
        ISender sender)
    {
        var result = await sender.Send(request);
        return result.Match(
            response => TypedResults.Created("/Tags", response),
            errors => Results.Problem());
    }
}