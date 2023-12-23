using ErrorOr;
using MediatR;

namespace Chores.Application.Modules.Chores.Iterations;

public static class CreateChoreIteration
{
    public sealed record Request(
        Guid ChoreId,
        DateOnly Date,
        string? Notes
    ) : IRequest<ErrorOr<Created>>;
}