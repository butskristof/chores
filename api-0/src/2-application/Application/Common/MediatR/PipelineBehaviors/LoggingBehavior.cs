using MediatR;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Common.MediatR.PipelineBehaviors;

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
        var start = _timeProvider.GetTimestamp();
        try
        {
            response = await next();
        }
        finally
        {
            var end = _timeProvider.GetTimestamp();
            var diff = _timeProvider.GetElapsedTime(start, end);
            _logger.LogDebug("Completed request {RequestName} in {ElapsedMilliseconds} ms",
                requestName, diff.TotalMilliseconds);
        }

        return response;
    }
}