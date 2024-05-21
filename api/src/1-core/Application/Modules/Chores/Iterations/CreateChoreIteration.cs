using Chores.Application.Common.FluentValidation;
using Chores.Application.Common.Persistence;
using Chores.Domain.Models.Chores;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Modules.Chores.Iterations;

public static class CreateChoreIteration
{
    public sealed record Request(
        Guid ChoreId,
        DateOnly Date,
        string? Notes
    ) : IRequest<ErrorOr<Response>>;

    public sealed record Response(Guid Id);

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator(TimeProvider timeProvider)
        {
            RuleFor(r => r.Date)
                .Must(input =>
                {
                    // we aim to support time zones partially: the passed in date should already have begun
                    // *somewhere* in the world. As an easy validation, we start from the current UTC date and port it 
                    // to the "earliest" (most in the east) time zone and use that as a reference for the current date
                    
                    var now = timeProvider.GetUtcNow();
                    // UTC+14:00 is regarding as the largest possible offset from UTC
                    // https://en.wikipedia.org/wiki/UTC%2B14:00
                    var earliest = now.ToOffset(TimeSpan.FromHours(14));
                    var currentDay = DateOnly.FromDateTime(earliest.Date);
                    return input <= currentDay;
                })
                .WithMessage(ErrorCodes.Invalid);
        }
    }

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
            _logger.LogDebug("Handling CreateChoreIteration request");

            var chore = await _db
                .CurrentUserChores(true)
                .Include(c => c.Iterations)
                .SingleOrDefaultAsync(c => c.Id == request.ChoreId, cancellationToken);
            if (chore is null)
            {
                _logger.LogDebug("Chore with ID {Id} was not found in database or does not belong to this user",
                    request.ChoreId);
                return Error.NotFound(nameof(request.ChoreId), $"Could not find Chore with id {request.ChoreId}");
            }

            _logger.LogDebug("Fetched chore with iterations from database");

            var iteration = new ChoreIteration
            {
                ChoreId = chore.Id,
                Date = request.Date,
                Notes = request.Notes,
            };
            _logger.LogDebug("Mapped request to entity");

            chore.Iterations.Add(iteration);
            await _db.SaveChangesAsync(CancellationToken.None);
            _logger.LogDebug("Persisted new entity to database");

            return new Response(iteration.Id);
        }
    }
}