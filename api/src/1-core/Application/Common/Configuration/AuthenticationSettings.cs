using Chores.Application.Common.FluentValidation;
using FluentValidation;

namespace Chores.Application.Common.Configuration;

public sealed record AuthenticationSettings
{
    public required string Issuer { get; init; }
    public required string[] Audiences { get; init; }
}

internal sealed class AuthenticationSettingsValidator : AbstractValidator<AuthenticationSettings>
{
    public AuthenticationSettingsValidator()
    {
        RuleFor(r => r.Issuer)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ErrorCodes.Required)
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage(ErrorCodes.Invalid);

        RuleFor(r => r.Audiences)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage(ErrorCodes.Required)
            .NotEmpty()
            .WithMessage(ErrorCodes.Required);
    }
}