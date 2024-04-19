using Chores.Application.Common.FluentValidation;
using FluentValidation;

namespace Chores.Application.Common.Configuration;

public sealed record ClientSettings
{
    public required string[] ClientUrls { get; init; }
}

internal sealed class ClientSettingsValidator : AbstractValidator<ClientSettings>
{
    public ClientSettingsValidator()
    {
        RuleFor(r => r.ClientUrls)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithErrorCode(ErrorCodes.Required);

        RuleForEach(r => r.ClientUrls)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode(ErrorCodes.Required)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithErrorCode(ErrorCodes.Invalid);
    }
}