using Chores.Domain.Common;
using Chores.Domain.Models.Chores;

namespace Chores.Domain.Models.Tags;

public sealed class Tag : BaseAuditableEntity
{
    public required string Name { get; set; }
    public string? Color { get; set; }
    public string? Icon { get; set; }
    public const int IconMaxLength = 32;

    public List<Chore> Chores { get; } = new(); // "skip navigation"
    public List<ChoreTag> ChoreTags { get; set; } = new();
}