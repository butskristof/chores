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
            .Url();

        RuleFor(r => r.Audiences)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage(ErrorCodes.Required)
            .NotEmpty()
            .WithMessage(ErrorCodes.Required);

        RuleForEach(r => r.Audiences)
            .NotEmpty()
            .WithMessage(ErrorCodes.Invalid);
    }
}