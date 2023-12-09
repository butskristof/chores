using Chores.Application.Common.Constants;
using FluentValidation;

namespace Chores.Application.Common.FluentValidation;

internal static class FluentValidationExtensions
{
    private static IRuleBuilder<T, TProperty> NotEmptyWithErrorCode<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder)
        => ruleBuilder
            .NotEmpty()
            .WithMessage(ErrorCodes.Required);

    internal static IRuleBuilder<T, string> ValidString<T>(this IRuleBuilder<T, string> ruleBuilder,
        int maxLength = ApplicationConstants.DefaultMaxStringLength)
        => ruleBuilder
            .NotEmptyWithErrorCode()
            .MaximumLength(maxLength)
            .WithMessage(ErrorCodes.Invalid);
}