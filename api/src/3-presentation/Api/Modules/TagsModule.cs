using Chores.Api.Extensions;
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
            .WithName(nameof(GetTags))
            .ProducesOk<GetTags.Response>();

        group
            .MapPost("", CreateTag)
            .WithName(nameof(CreateTag))
            .ProducesCreated<CreateTag.Response>()
            .ProducesValidationProblem();

        return endpoints;
    }

    private static async Task<IResult> GetTags(ISender sender)
        => (await sender.Send(new GetTags.Request())).MapToValueOrProblem(TypedResults.Ok);

    private static async Task<IResult> CreateTag([FromBody] CreateTag.Request request, ISender sender)
    {
        var result = await sender.Send(request);
        return result.MapToValueOrProblem(response => TypedResults.Created($"/Tags/{response.Id}", response));
    }
}