using ErrorOr;
using MediatR;

namespace Chores.Application.Modules.Chores.Iterations;

public static class DeleteChoreIteration
{
    public sealed record Request(
        Guid ChoreId,
        Guid IterationId
    ) : IRequest<ErrorOr<Deleted>>;
}