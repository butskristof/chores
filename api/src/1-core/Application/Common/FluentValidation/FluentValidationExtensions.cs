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
        bool required = true,
        int maxLength = ApplicationConstants.DefaultMaxStringLength)
    {
        if (required)
            ruleBuilder = ruleBuilder.NotEmptyWithErrorCode();
        
        ruleBuilder = ruleBuilder
            .MaximumLength(maxLength)
            .WithMessage(ErrorCodes.Invalid);
        
        return ruleBuilder;
    }

    internal static IRuleBuilder<T, int> PositiveInteger<T>(this IRuleBuilder<T, int> ruleBuilder,
        bool zeroInclusive)
        => (zeroInclusive
                ? ruleBuilder
                    .GreaterThanOrEqualTo(0)
                : ruleBuilder
                    .GreaterThan(0))
            .WithMessage(ErrorCodes.Invalid);
}