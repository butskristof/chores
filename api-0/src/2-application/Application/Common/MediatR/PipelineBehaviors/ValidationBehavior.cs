using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Common.MediatR.PipelineBehaviors;

internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
{
    #region construction

    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(ILogger<ValidationBehavior<TRequest, TResponse>> logger,
        IEnumerable<IValidator<TRequest>> validators)
    {
        _logger = logger;
        _validators = validators;
    }

    #endregion

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).FullName;
        _logger.LogDebug("Validating incoming request {RequestName}", requestName);

        if (!_validators.Any())
        {
            _logger.LogDebug("No validators found");
            return await next();
        }

        _logger.LogDebug("Applying {Count} validators", _validators.Count());

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = results
            .Where(result => result.Errors.Any())
            .SelectMany(e => e.Errors)
            .ToList();

        if (failures.Any())
        {
            _logger.LogDebug("Validation resulted in {Count} failures", failures.Count);
            return (dynamic)failures;
        }

        _logger.LogDebug("Validation passed successfully");
        return await next();
    }
}