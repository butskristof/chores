namespace Chores.Bff.Configuration;

internal sealed class AuthenticationSettings
{
    internal const string SectionName = "Authentication";
    
    public required string Authority { get; init; }
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
}