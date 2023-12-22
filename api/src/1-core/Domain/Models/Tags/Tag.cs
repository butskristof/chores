using Chores.Domain.Common;
using Chores.Domain.Models.Chores;

namespace Chores.Domain.Models.Tags;

public sealed class Tag : BaseAuditableEntity
{
    public required string Name { get; set; }

    public List<Chore> Chores { get; } = new(); // "skip navigation"
    public List<ChoreTag> ChoreTags { get; set; } = new();
}