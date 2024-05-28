namespace Chores.Bff.Configuration;

internal sealed class FrontendSettings
{
    internal const string SectionName = "Frontend";

    public string? ClientUrl { get; init; }
    public Uri? ClientUri => Uri.TryCreate(ClientUrl, UriKind.Absolute, out var uri) ? uri : null;
}