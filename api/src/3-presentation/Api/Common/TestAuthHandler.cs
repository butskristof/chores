using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Chores.Api.Common;

// https://stackoverflow.com/questions/73173839/adding-a-fake-user-when-developing

internal sealed class TestAuthHandlerOptions : AuthenticationSchemeOptions;

internal sealed class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
{
    internal const string AuthenticationScheme = "TEST";

    public TestAuthHandler(IOptionsMonitor<TestAuthHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "butskristof")
        };
        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);
        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }
}