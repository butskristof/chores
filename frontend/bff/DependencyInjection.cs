using Chores.Bff.Configuration;
using Chores.Bff.Services;
using Duende.Bff;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace Chores.Bff;

internal static class DependencyInjection
{
    #region Configuration

    internal static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        services
            .AddValidatedSettings<AuthenticationSettings>(AuthenticationSettings.SectionName)
            .AddValidatedSettings<RemoteApiSettings>(RemoteApiSettings.SectionName)
            .AddValidatedSettings<FrontendSettings>(FrontendSettings.SectionName);
        return services;
    }

    private static IServiceCollection AddValidatedSettings<TOptions>(this IServiceCollection services,
        string sectionName)
        where TOptions : class
    {
        services
            .AddOptions<TOptions>()
            .BindConfiguration(sectionName);

        return services;
    }

    #endregion

    #region BFF Host

    internal static IServiceCollection AddChoresBff(this IServiceCollection services, bool isDevelopment)
    {
        services
            .AddBff()
            .AddRemoteApis();
        services.AddTransient<IReturnUrlValidator, FrontendHostReturnUrlValidator>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = false;
                options.Cookie.Name = "__Host-bff";
                options.Cookie.SameSite = SameSiteMode.Strict;
            })
            .AddOpenIdConnect(options =>
            {
                using var serviceProvider = services.BuildServiceProvider();
                var configuration = serviceProvider
                    .GetRequiredService<IOptions<AuthenticationSettings>>()
                    .Value;
                
                options.Authority = configuration.Authority;
                options.ClientId = configuration.ClientId;
                options.ClientSecret = configuration.ClientSecret;
                options.ResponseType = "code";
                options.ResponseMode = "query"; // compatible with SameSite=Strict
                options.MapInboundClaims = false; // don't map JWT claims to .NET claims
                options.GetClaimsFromUserInfoEndpoint = true;
                // save tokens into authentication sessions to enable automatic token mgmt
                options.SaveTokens = true;
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("offline_access"); // add refresh token
            });
        services
            .AddAuthorization();

        return services;
    }

    #endregion
}