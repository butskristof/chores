using Chores.Application.Common.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Chores;

public static class GetChores
{
    public sealed record Request : IRequest<ErrorOr<Response>>;

    public sealed record Response
    {
        public Response(IEnumerable<ChoreDto> chores)
        {
            Chores = chores;
        }

        public IEnumerable<ChoreDto> Chores { get; }

        public sealed record ChoreDto(
            Guid Id,
            string Name,
            int Interval,
            IEnumerable<TagDto> Tags,
            DateOnly? LastIteration);

        public sealed record TagDto(Guid Id, string Name);
    }

    internal sealed class Handler : IRequestHandler<Request, ErrorOr<Response>>
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
            _logger.LogDebug("Handling GetChores request");

            var chores = await _db
                .CurrentUserChores(false)
                .Select(c =>
                    new Response.ChoreDto(c.Id, c.Name, c.Interval,
                        c.Tags.Select(t => new Response.TagDto(t.Id, t.Name)),
                        c.Iterations.Max(i => i.Date)))
                .ToListAsync(cancellationToken);
            _logger.LogDebug("Fetched all tags from database as DTO");

            return new Response(chores);
        }
    }
}