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
            .NotNull()
            .WithMessage(ErrorCodes.Required);

        RuleForEach(r => r.ClientUrls)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ErrorCodes.Invalid)
            .Url();
    }
}