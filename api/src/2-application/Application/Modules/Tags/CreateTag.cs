using Chores.Application.Common.Persistence;
using Chores.Domain.Models.Tags;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Tags;

public static class CreateTag
{
    public sealed record Request(string Name) : IRequest<Response>;

    public sealed record Response(Guid Id, string Name);

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.Name)
                .NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Request, Response>
    {
        #region construction

        private readonly ILogger<Handler> _logger;
        private readonly IAppDbContext _context;

        public Handler(ILogger<Handler> logger, IAppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        #endregion

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var tag = new Tag { Name = request.Name };
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync(CancellationToken.None);
            
            return new Response(tag.Id, tag.Name);
        }
    }
}