using Chores.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Tags;

public static class CreateTag
{
    public sealed record Request(string Name) : IRequest<Response>;

    public sealed record Response(Guid Id, string Name);

    internal class Handler : IRequestHandler<Request, Response>
    {
        #region construction

        private readonly ILogger<Handler> _logger;

        public Handler(ILogger<Handler> logger)
        {
            _logger = logger;
        }

        #endregion

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            await Task.Delay(1, cancellationToken);
            var entity = new Tag
            {
                Id = Guid.Empty,
                Name = request.Name,
            };
            return new Response(entity.Id, entity.Name);
        }
    }
}