using Chores.Application.Common.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Tags;

public static class DeleteTag
{
    public sealed record Request(Guid Id) : IRequest<ErrorOr<Deleted>>;

    internal sealed class Handler : IRequestHandler<Request, ErrorOr<Deleted>>
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
            _logger.LogDebug("Handling DeleteTag request");

            var tag = await _db
                .CurrentUserTags(true)
                .Include(t => t.Chores)
                .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (tag is null)
            {
                _logger.LogDebug("Tag with ID {Id} was not found in database or does not belong to this user",
                    request.Id);
                return Error.NotFound(nameof(request.Id), $"Could not find Tag with id {request.Id}");
            }
            _logger.LogDebug("Fetched entity from database");

            if (tag.Chores.Count != 0)
                return Error.Validation("Chores");

            _db.Tags.Remove(tag);
            await _db.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Removed entity from database");
            // TODO handle failing delete because of FK constraints

            return Result.Deleted;
        }
    }
}