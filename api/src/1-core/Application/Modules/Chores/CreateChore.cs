using Chores.Application.Common.Persistence;
using Chores.Domain.Models.Chores;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Chores;

public static class CreateChore
{
    public sealed record Request(
        string Name,
        int Interval
    ) : IRequest<ErrorOr<Response>>;

    public sealed record Response(Guid Id, string Name, int Interval);

    internal class Handler : IRequestHandler<Request, ErrorOr<Response>>
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

        public async Task<ErrorOr<Response>> Handle(Request request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Creating a new Chore");
            
            var chore = new Chore { Name = request.Name, Interval = request.Interval };
            _logger.LogDebug("Mapped request to entity");

            _db.Chores.Add(chore);
            await _db.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Persisted new entity to database");

            var response = new Response(chore.Id, chore.Name, chore.Interval);
            _logger.LogDebug("Mapped entity to response");

            return response;
        }
    }
}