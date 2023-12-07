using MediatR;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Common.MediatR.PipelineBehaviors;

// this behavior should wrap the entire MediatR pipeline
// it is used to log incoming requests and the time required to get to a response
// logs should be restricted to Debug level

internal sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
{
    #region construction

    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly TimeProvider _timeProvider;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, TimeProvider timeProvider)
    {
        _logger = logger;
        _timeProvider = timeProvider;
    }

    #endregion

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).FullName;
        _logger.LogDebug("Incoming request: {RequestName}", requestName);

        TResponse response;
        // keep a record of when we start processing the request
        var start = _timeProvider.GetTimestamp();
        try
        {
            // continue in the pipeline
            response = await next();
        }
        finally
        {
            // these steps belong in the finally, so even in case an exception occurs, we can still close our logging
            var end = _timeProvider.GetTimestamp(); // stop the timer
            var diff = _timeProvider.GetElapsedTime(start, end); // calculate the time spent
            _logger.LogDebug("Completed request {RequestName} in {ElapsedMilliseconds} ms",
                requestName, diff.TotalMilliseconds);
        }

        return response;
    }
}