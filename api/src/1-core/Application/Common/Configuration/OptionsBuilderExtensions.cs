using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Chores.Application.Common.Configuration;

public static class OptionsBuilderExtensions
{
    public static OptionsBuilder<TOptions> FluentValidateOptions<TOptions>(this OptionsBuilder<TOptions> builder)
        where TOptions : class
    {
        builder.Services
            .AddSingleton<IValidateOptions<TOptions>>(sp =>
                new FluentValidateOptions<TOptions>(sp, builder.Name));

        return builder;
    }
}