using Chores.Application.Common.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Chores;

public static class UpdateChoreNotes
{
    public sealed record Request(
        Guid ChoreId,
        string? Notes
    ) : IRequest<ErrorOr<Updated>>;

    internal class Handler : IRequestHandler<Request, ErrorOr<Updated>>
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
            _logger.LogDebug("Handling UpdateChoreNotes request");

            var chore = await _db
                .CurrentUserChores(true)
                .SingleOrDefaultAsync(c => c.Id == request.ChoreId, cancellationToken);
            if (chore is null)
            {
                _logger.LogDebug("Chore with ID {Id} was not found in database or does not belong to this user",
                    request.ChoreId);
                return Error.NotFound(nameof(request.ChoreId), $"Could not find Chore with id {request.ChoreId}");
            }

            _logger.LogDebug("Fetched entity from database");

            // TODO trim & replace w/ null
            chore.Notes = request.Notes;
            _logger.LogDebug("Applied changes from request to entity");

            await _db.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Persisted changes to database");

            return Result.Updated;
        }
    }
}