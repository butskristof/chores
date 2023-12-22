using Chores.Api.Extensions;
using Chores.Application.Modules.Chores;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chores.Api.Modules;

internal static class ChoresModule
{
    internal static IEndpointRouteBuilder MapChoresEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/chores")
            .WithTags("Chores")
            .RequireAuthorization();

        group
            .MapGet("", GetChores)
            .WithName(nameof(GetChores))
            .ProducesOk<GetChores.Response>();

        group
            .MapPost("", CreateChore)
            .WithName(nameof(CreateChore))
            .ProducesCreated<CreateChore.Response>()
            .ProducesValidationProblem();

        group
            .MapGet("{ChoreId:guid}", GetChore)
            .WithName(nameof(GetChore))
            .ProducesOk<GetChore.Response>()
            .ProducesNotFound();

        group
            .MapPut("{ChoreId:guid}", UpdateChore)
            .WithName(nameof(UpdateChore))
            .ProducesNoContent()
            .ProducesValidationProblem()
            .ProducesNotFound();

        group
            .MapDelete("{ChoreId:guid}", DeleteChore)
            .WithName(nameof(DeleteChore))
            .ProducesNoContent()
            .ProducesNotFound();

        group
            .MapPut("{ChoreId:guid}/Notes", UpdateChoreNotes)
            .WithName(nameof(UpdateChoreNotes))
            .ProducesNoContent()
            .ProducesNotFound();

        group
            .MapPut("{ChoreId:guid}/Tags", UpdateChoreTags)
            .WithName(nameof(UpdateChoreTags))
            .ProducesNoContent()
            .ProducesNotFound();

        return endpoints;
    }

    private static async Task<IResult> GetChores(ISender sender)
        => (await sender.Send(new GetChores.Request())).MapToValueOrProblem(TypedResults.Ok);

    private static async Task<IResult> CreateChore([FromBody] CreateChore.Request request, ISender sender)
    {
        var result = await sender.Send(request);
        return result.MapToValueOrProblem(response => TypedResults.Created($"/Chores/{response.Id}", response));
    }

    private static async Task<IResult> GetChore([FromRoute] Guid ChoreId, ISender sender)
        => (await sender.Send(new GetChore.Request(ChoreId))).MapToValueOrProblem(TypedResults.Ok);

    private static async Task<IResult> UpdateChore([FromBody] UpdateChore.Request request, ISender sender)
        => (await sender.Send(request)).MapToValueOrProblem(_ => TypedResults.NoContent());

    private static async Task<IResult> DeleteChore([FromRoute] Guid ChoreId, ISender sender)
        => (await sender.Send(new DeleteChore.Request(ChoreId))).MapToValueOrProblem(_ => TypedResults.NoContent());

    private static async Task<IResult> UpdateChoreNotes([FromBody] UpdateChoreNotes.Request request, ISender sender)
        => (await sender.Send(request)).MapToValueOrProblem(_ => TypedResults.NoContent());

    private static async Task<IResult> UpdateChoreTags([FromBody] UpdateChoreTags.Request request, ISender sender)
        => (await sender.Send(request)).MapToValueOrProblem(_ => TypedResults.NoContent());
}