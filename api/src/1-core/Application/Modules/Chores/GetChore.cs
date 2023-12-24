using Chores.Application.Common.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Chores;

public static class GetChore
{
    public sealed record Request(Guid Id) : IRequest<ErrorOr<Response>>;

    public sealed record Response(
        Guid Id,
        string Name,
        int Interval,
        string? Notes,
        IEnumerable<TagDto> Tags,
        IEnumerable<IterationDto> Iterations
    );

    public sealed record TagDto(Guid Id, string Name);

    public sealed record IterationDto(Guid Id, DateOnly Date, string? Notes);

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
            _logger.LogDebug("Handling GetChore request");

            var chore = await _db
                .CurrentUserChores(false)
                .Include(c => c.Tags)
                .Include(c => c.Iterations)
                .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (chore is null)
            {
                _logger.LogDebug("Chore with ID {Id} was not found in database or does not belong to this user",
                    request.Id);
                return Error.NotFound(nameof(request.Id), $"Could not find Chore with id {request.Id}");
            }

            _logger.LogDebug("Fetched entity from database");

            var dto = new Response(chore.Id, chore.Name, chore.Interval, chore.Notes,
                chore.Tags.Select(t => new TagDto(t.Id, t.Name)),
                chore.Iterations.Select(i => new IterationDto(i.Id, i.Date, i.Notes)));
            _logger.LogDebug("Mapped entity to DTO");

            return dto;
        }
    }
}