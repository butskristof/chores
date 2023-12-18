using ErrorOr;
using MediatR;

namespace Chores.Application.Modules.Chores;

public static class UpdateChoreTags
{
    public sealed record Request : IRequest<ErrorOr<Updated>>;
}