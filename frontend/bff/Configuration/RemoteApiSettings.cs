namespace Chores.Bff.Configuration;

internal sealed class RemoteApiSettings
{
    internal const string SectionName = "RemoteApis";
    
    public required string ChoresApiUrl { get; init; }
}