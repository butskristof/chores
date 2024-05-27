namespace Chores.Bff.Configuration;

internal sealed class FrontendSettings
{
    internal const string SectionName = "Frontend";

    public string? DevServerUrl { get; init; }
    public Uri? DevServerUri => Uri.TryCreate(DevServerUrl, UriKind.Absolute, out var uri) ? uri : null;
}