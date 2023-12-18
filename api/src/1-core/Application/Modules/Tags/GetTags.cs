using Chores.Application.Common.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Tags;

public static class GetTags
{
    public sealed record Request : IRequest<ErrorOr<Response>>;

    public sealed record Response
    {
        public IEnumerable<TagDto> Tags { get; set; }

        public Response(IEnumerable<TagDto> tags)
        {
            Tags = tags;
        }

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
            var tags = await _db
                .CurrentUserTags(false)
                .Select(t => new Response.TagDto(t.Id, t.Name))
                .ToListAsync(cancellationToken);
            _logger.LogDebug("Fetched all tags from database as DTO");
            
            return new Response(tags);
        }
    }
}