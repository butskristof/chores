using Chores.Application.Common.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Chores.Iterations;

public static class DeleteChoreIteration
{
    public sealed record Request(
        Guid ChoreId,
        Guid IterationId
    ) : IRequest<ErrorOr<Deleted>>;

    internal class Handler : IRequestHandler<Request, ErrorOr<Deleted>>
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

        public async Task<ErrorOr<Deleted>> Handle(Request request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Handling DeleteChoreIteration request");

            var chore = await _db
                .CurrentUserChores(true)
                .Include(c => c.Iterations)
                .SingleOrDefaultAsync(c => c.Id == request.ChoreId, cancellationToken);
            if (chore is null)
            {
                _logger.LogDebug("Chore with ID {Id} was not found in database or does not belong to this user",
                    request.ChoreId);
                return Error.NotFound(nameof(request.ChoreId), $"Could not find Chore with id {request.ChoreId}");
            }
            _logger.LogDebug("Fetched chore with iterations from database");

            var iteration = chore.Iterations.SingleOrDefault(i => i.Id == request.IterationId);
            if (iteration is null)
            {
                _logger.LogDebug("Iteration with ID {Id} was not found in the current Chore", request.IterationId);
                return Error.NotFound(nameof(request.IterationId),
                    $"Could not find Iteration with id {request.IterationId} in Chore with id {request.ChoreId}");
            }
            _logger.LogDebug("Found iteration in chore");

            chore.Iterations.Remove(iteration);
            await _db.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Removed iteration from chore and persisted changes to database");

            return Result.Deleted;
        }
    }
}