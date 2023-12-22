using Chores.Application.Common.Persistence;
using Chores.Domain.Models.Chores;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Chores;

public static class UpdateChoreTags
{
    public sealed record Request(
        Guid ChoreId,
        IEnumerable<Guid> TagIds
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
            _logger.LogDebug("Handling UpdateChoreTags request");

            var chore = await _db
                .CurrentUserChores(true)
                .Include(c => c.Tags)
                .SingleOrDefaultAsync(c => c.Id == request.ChoreId, cancellationToken);
            if (chore is null)
            {
                _logger.LogDebug("Chore with ID {Id} was not found in database or does not belong to this user",
                    request.ChoreId);
                return Error.NotFound(nameof(request.ChoreId), $"Could not find Chore with id {request.ChoreId}");
            }

            _logger.LogDebug("Fetched entity from database");

            var existingTagsCount = await _db
                .CurrentUserTags(false)
                .CountAsync(t => request.TagIds.Contains(t.Id), cancellationToken: cancellationToken);
            if (existingTagsCount != request.TagIds.Count())
                return Error.NotFound(nameof(request.TagIds), "Could not find Tag for each of the requested ids");

            chore.ChoreTags = request.TagIds.Select(id => new ChoreTag { TagId = id }).ToList();
            _logger.LogDebug("Replaced tags list on chore");

            await _db.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Persisted changes to database");

            return Result.Updated;
        }
    }
}