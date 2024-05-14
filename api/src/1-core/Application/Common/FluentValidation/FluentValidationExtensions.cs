using System.Text.RegularExpressions;
using Chores.Application.Common.Constants;
using FluentValidation;

namespace Chores.Application.Common.FluentValidation;

internal static class FluentValidationExtensions
{
    private static IRuleBuilderOptions<T, TProperty> NotEmptyWithErrorCode<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder)
        => ruleBuilder
            .NotEmpty()
            .WithMessage(ErrorCodes.Required);

    internal static IRuleBuilderOptions<T, string?> ValidString<T>(this IRuleBuilder<T, string?> ruleBuilder,
        bool required = true,
        int maxLength = ApplicationConstants.DefaultMaxStringLength)
    {
        if (required)
            ruleBuilder = ruleBuilder.NotEmptyWithErrorCode();

        return ruleBuilder
            .MaximumLength(maxLength)
            .WithMessage(ErrorCodes.Invalid);
    }

    internal static IRuleBuilderOptions<T, int> PositiveInteger<T>(this IRuleBuilder<T, int> ruleBuilder,
        bool zeroInclusive)
        => (zeroInclusive
                ? ruleBuilder
                    .GreaterThanOrEqualTo(0)
                : ruleBuilder
                    .GreaterThan(0))
            .WithMessage(ErrorCodes.Invalid);

    internal static IRuleBuilderOptions<T, string> Url<T>(this IRuleBuilder<T, string> ruleBuilder)
        => ruleBuilder
            .Must(value => Uri.TryCreate(value, UriKind.Absolute, out var uri) &&
                           (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            .WithMessage(ErrorCodes.Invalid);

    private static readonly Regex HexColorRegex = new(@"^#(([0-9a-fA-F]{2}){3})$", RegexOptions.Compiled);
    internal static IRuleBuilderOptions<T, string?> HexColor<T>(this IRuleBuilder<T, string?> ruleBuilder)
        => ruleBuilder
            .Matches(HexColorRegex)
            .WithMessage(ErrorCodes.Invalid);
}