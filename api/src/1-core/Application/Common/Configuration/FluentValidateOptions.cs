using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chores.Application.Common.Configuration;

internal sealed class FluentValidateOptions<TOptions> : IValidateOptions<TOptions>
    where TOptions : class
{
    #region construction

    // private readonly ILogger<FluentValidateOptions<TOptions>> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly string? _name;

    public FluentValidateOptions(
        // ILogger<FluentValidateOptions<TOptions>> logger, 
        IServiceProvider serviceProvider,
        string? name)
    {
        // _logger = logger;
        _serviceProvider = serviceProvider;
        _name = name;
    }

    #endregion

    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        // _logger.LogDebug("Applying FluentValidation to options {Name}", name);

        if (_name is not null && _name != name) return ValidateOptionsResult.Skip;
        ArgumentNullException.ThrowIfNull(options);

        using var scope = _serviceProvider.CreateScope();
        var validator = scope.ServiceProvider.GetRequiredService<IValidator<TOptions>>();
        var validationResult = validator.Validate(options);
        
        if (validationResult.IsValid) return ValidateOptionsResult.Success;

        var errors = validationResult
            .Errors
            .Select(f => $"{f.PropertyName}: {f.ErrorMessage}");
        return ValidateOptionsResult.Fail(errors);
    }
}