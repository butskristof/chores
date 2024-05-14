using Chores.Application.Common.FluentValidation;
using Chores.Application.Common.Persistence;
using Chores.Domain.Models.Tags;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Tags;

public static class CreateTag
{
    public sealed record Request(
        string Name,
        string? Color,
        string? Icon
    ) : IRequest<ErrorOr<Response>>;

    public sealed record Response(
        Guid Id,
        string Name,
        string? Color,
        string? Icon
    );

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Name)
                .ValidString();

            RuleFor(r => r.Color)
                .Cascade(CascadeMode.Stop)
                .ValidString(required: false)
                .HexColor()
                .Unless(r => string.IsNullOrWhiteSpace(r.Color));

            RuleFor(r => r.Icon)
                .Cascade(CascadeMode.Stop)
                .ValidString(required: false, maxLength: Tag.IconMaxLength);
        }
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
            _logger.LogDebug("Creating a new Tag");

            if (await _db.CurrentUserTags(false).AnyAsync(t => t.Name == request.Name, cancellationToken))
                return Error.Conflict(nameof(request.Name));

            var tag = new Tag
            {
                Name = request.Name.Trim(),
                Color = string.IsNullOrWhiteSpace(request.Color) ? null : request.Color.Trim().ToLowerInvariant(),
                Icon = string.IsNullOrWhiteSpace(request.Icon) ? null : request.Icon.Trim().ToLowerInvariant(),
            };
            _logger.LogDebug("Mapped request to entity");

            _db.Tags.Add(tag);
            await _db.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Persisted new entity to database");

            var response = new Response(tag.Id, tag.Name, tag.Color, tag.Icon);
            _logger.LogDebug("Mapped entity to response");

            return response;
        }
    }
}