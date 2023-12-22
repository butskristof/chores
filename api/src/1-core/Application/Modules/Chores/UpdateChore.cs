using Chores.Application.Common.FluentValidation;
using Chores.Application.Common.Persistence;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Chores;

public static class UpdateChore
{
    public sealed record Request(
        Guid Id,
        string Name,
        int Interval
    ) : IRequest<ErrorOr<Updated>>;

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Name)
                .ValidString();

            RuleFor(r => r.Interval)
                .PositiveInteger(false);
        }
    }

    internal sealed class Handler : IRequestHandler<Request, ErrorOr<Updated>>
    {
        #region construction

        private readonly ILogger<Handler> _logger;
        private readonly IAppDbContext _db;

        public Handler(ILogger<Handler> logger, IAppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        #endregion

        public async Task<ErrorOr<Updated>> Handle(Request request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Handling DeleteChore request");

            var chore = await _db
                .CurrentUserChores(true)
                .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (chore is null)
            {
                _logger.LogDebug("Chore with ID {Id} was not found in database or does not belong to this user",
                    request.Id);
                return Error.NotFound(nameof(request.Id), $"Could not find Chore with id {request.Id}");
            }

            _logger.LogDebug("Fetched entity from database");

            chore.Name = request.Name;
            chore.Interval = request.Interval;
            _logger.LogDebug("Applied changes from request to entity");

            await _db.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Persisted changes to database");

            return Result.Updated;
        }
    }
}