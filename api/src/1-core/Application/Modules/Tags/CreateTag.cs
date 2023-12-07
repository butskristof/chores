using Chores.Domain.Models;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Tags;

public static class CreateTag
{
    public sealed record Request(string Name) : IRequest<ErrorOr<Response>>;

    public sealed record Response(Guid Id, string Name);

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Name)
                .NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Request, ErrorOr<Response>>
    {
        #region construction

        private readonly ILogger<Handler> _logger;

        public Handler(ILogger<Handler> logger)
        {
            _logger = logger;
        }

        #endregion

        public async Task<ErrorOr<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            if (request.Name == "hey")
                return Error.NotFound(description: "something not found");
            _logger.LogDebug("Creating a new Tag");

            await Task.Delay(1, cancellationToken);
            var entity = new Tag
            {
                Id = Guid.Empty,
                Name = request.Name,
            };
            _logger.LogDebug("Mapped request to entity");

            // TODO persist

            var response = new Response(entity.Id, entity.Name);
            _logger.LogDebug("Mapped entity to response");
            return response;
        }
    }
}