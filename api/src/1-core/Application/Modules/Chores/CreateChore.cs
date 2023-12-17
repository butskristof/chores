using ErrorOr;
using MediatR;

namespace Chores.Application.Modules.Chores;

public static class CreateChore
{
    public sealed record Request(
        string Name,
        int Interval
    ) : IRequest<ErrorOr<Response>>;

    public sealed record Response(Guid Id, string Name, int Interval);
}