using Chores.Application.Common.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Chores;

public static class DeleteChore
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

            _db.Chores.Remove(chore);
            await _db.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Removed entity from database");

            return Result.Deleted;
        }
    }
}