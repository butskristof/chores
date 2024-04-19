using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chores.Application.Common.FluentValidation;

// this implementation of IValidateOptions will help in applying FluentValidation validators
// to IOptions objects. By creating registering an instance for TOptions in the DI container all the 
// registered validators will be executed, and a failing result will be returned in case of non-compliant
// objects 

// since IValidateOptions is executed by IOptionsMonitor, which is registered with singleton, all instances
// of this implementation should be registered with singleton scope as well 
// hence why the constructor takes in a service provider: a scope will be created in the Validate method

// keep in mind the Validate method in IValidateOptions is synchronous, so the validators that are implemented
// should only use synchronous validators as well

internal sealed class FluentValidateOptions<TOptions> : IValidateOptions<TOptions>
    where TOptions : class
{
    #region construction

    // IValidateOptions implementations should be registered with singleton scope
    private readonly IServiceProvider _serviceProvider;

    // support named options
    private readonly string? _name;

    public FluentValidateOptions(IServiceProvider serviceProvider, string? name)
    {
        _serviceProvider = serviceProvider;
        _name = name;
    }

    #endregion

    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        var logger = _serviceProvider.GetRequiredService<ILogger<FluentValidateOptions<TOptions>>>();
        var typeName = typeof(TOptions).Name;

        if (string.IsNullOrWhiteSpace(name))
            logger.LogDebug("Validating options of type {OptionsType}", typeName);
        else
            logger.LogDebug("Validating options of type {OptionsType} with name {Name}", typeof(TOptions).Name, name);

        if (_name is not null && _name != name)
        {
            logger.LogDebug("Injected name {Name} does not match, returning 'Skip' result", _name);
            return ValidateOptionsResult.Skip;
        }

        using var scope = _serviceProvider.CreateScope();
        var validators = scope.ServiceProvider
            .GetServices<IValidator<TOptions>>()
            .ToList();
        logger.LogDebug("Found {Count} validator(s) to apply for {TypeName}", validators.Count, typeName);

        var results = validators
            .Select(v => v.Validate(options))
            .ToList();

        if (results.All(r => r.IsValid))
        {
            logger.LogDebug("Configured values for {TypeName} successfully passed validation", typeName);
            return ValidateOptionsResult.Success;
        }

        var errors = results
            .SelectMany(r => r.Errors)
            .Select(f => $"{f.PropertyName}: {f.ErrorMessage}")
            .ToList();

        logger.LogDebug("Validation failed with {Count} errors", errors.Count);
        return ValidateOptionsResult.Fail(errors);
    }
}