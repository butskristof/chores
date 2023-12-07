using Chores.Application.Common.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Tags;

public static class DeleteTag
{
    public sealed record Request(Guid Id) : IRequest<ErrorOr<Deleted>>;

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
            var tag = await _db.Tags.SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (tag == null) return Error.NotFound(description: $"Could not find Tag with id {request.Id}");
            _logger.LogDebug("Fetched entity from database");

            _db.Tags.Remove(tag);
            await _db.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Removed entity from database");
            // TODO handle failing delete because of FK constraints

            return Result.Deleted;
        }
    }
}