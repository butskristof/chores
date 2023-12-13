using Chores.Application.Common.FluentValidation;
using Chores.Application.Common.Persistence;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Tags;

public static class UpdateTag
{
    public sealed record Request(
        Guid Id,
        string Name
    ) : IRequest<ErrorOr<Updated>>;

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Name)
                .ValidString();
        }
    }

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
            _logger.LogDebug("Updating Tag");

            var tag = await _db
                .CurrentUserTags(true)
                .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (tag is null)
                return Error.NotFound(nameof(tag.Id), $"Could not find Tag with id {request.Id}");
            _logger.LogDebug("Fetched entity from database");

            if (await _db.CurrentUserTags(false)
                    .AnyAsync(t => t.Name == request.Name && t.Id != tag.Id, cancellationToken))
                return Error.Conflict(nameof(request.Name));

            tag.Name = request.Name;
            _logger.LogDebug("Applied changes from request to entity");

            await _db.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Persisted changes to database");

            return Result.Updated;
        }
    }
}