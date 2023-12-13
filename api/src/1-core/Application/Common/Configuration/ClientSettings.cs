namespace Chores.Application.Common.Configuration;

public sealed record ClientSettings
{
    public string[] ClientUrls { get; init; } = Array.Empty<string>();
}