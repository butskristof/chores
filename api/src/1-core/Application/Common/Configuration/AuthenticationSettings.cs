namespace Chores.Application.Common.Configuration;

public sealed record AuthenticationSettings
{
    public string? Issuer { get; init; }
    public string[]? Audiences { get; init; }
}