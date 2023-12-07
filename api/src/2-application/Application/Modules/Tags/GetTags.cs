using Chores.Application.Common.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Tags;

public static class GetTags
{
    public sealed record Request : IRequest<Response>;

    public sealed record Response
    {
        public required IEnumerable<TagDto> Tags { get; set; }

        public sealed record TagDto(Guid Id, string Name);
    };

    // internal sealed class Validator : AbstractValidator<Request>
    // {
    //     public Validator()
    //     {
    //     }
    // }

    internal sealed class Handler : IRequestHandler<Request, Response>
    {
        #region construction

        private readonly ILogger<Handler> _logger;
        private readonly IAppDbContext _dbContext;

        public Handler(ILogger<Handler> logger, IAppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        #endregion

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            await Task.Delay(1, cancellationToken);
            throw new NotImplementedException();
            // var tags = await _dbContext
            //     .Tags
            //     .Select(t => new Response.TagDto(t.Id, t.Name))
            //     .ToListAsync(cancellationToken);
            //
            // return new Response { Tags = tags };
        }
    }
}