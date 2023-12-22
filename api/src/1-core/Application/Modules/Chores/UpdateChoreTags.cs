using ErrorOr;
using MediatR;

namespace Chores.Application.Modules.Chores;

public static class UpdateChoreTags
{
    public sealed record Request(
        Guid ChoreId,
        IEnumerable<Guid> TagIds
    ) : IRequest<ErrorOr<Updated>>;
}