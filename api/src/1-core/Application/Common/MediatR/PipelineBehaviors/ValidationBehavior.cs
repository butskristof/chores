using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Chores.Application.Common.MediatR.PipelineBehaviors;

// this behavior will retrieve all applicable validators from the DI container and verify whether the incoming 
// request passes all of them 
// in case it doesn't, the pipeline execution will be cut short and an error result will be returned

// keep in mind the validators registered with the FluentValidation library should mainly do input validation
// and not be concerned with business logic
// effectively, this behavior is intended to stop unprocessable requests from continuing further into the pipeline
// "an *invalid* request should never reach the handler"

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
        _logger.LogDebug("Validating incoming request {RequestName}", typeof(TRequest).FullName);

        if (!_validators.Any())
        {
            // in case no validators are found for the request type, just return out of the handler and
            // continue the pipeline
            _logger.LogDebug("No validators are available for request type");
            return await next();
        }

        _logger.LogDebug("Applying {Count} validators for request type", _validators.Count());

        // create a new validation context scoped to this execution
        var context = new ValidationContext<TRequest>(request);
        // run all validators asynchronously and collect the results
        var results = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        // aggregate failures from all validators into a single list
        var failures = results
            .Where(result => !result.IsValid)
            .SelectMany(result => result.Errors)
            .ToList();

        if (failures.Any())
        {
            // in case validation resulted in failures, return a result with the failures as errors
            // an exception should *not* be thrown: we're doing flow control here, and validation failures
            // are kind of expected (hence, not exceptional)
            _logger.LogDebug("Validation resulted in {Count} failures", failures.Count);
            throw new Exception(); // TODO factor out
        }

        // validation passed, continue in the pipeline as usual
        _logger.LogDebug("Validation passed successfully");
        return await next();
    }
}