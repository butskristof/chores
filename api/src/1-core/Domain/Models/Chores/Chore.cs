using Chores.Domain.Common;
using Chores.Domain.Models.Tags;

namespace Chores.Domain.Models.Chores;

public sealed class Chore : BaseAuditableEntity
{
    public required string Name { get; set; }
    public required int Interval { get; set; }

    public string? Notes { get; set; }

    public List<Tag> Tags { get; set; } = new(); // "skip navigation"
    public List<ChoreTag> ChoreTags { get; set; } = new();

    public List<ChoreIteration> Iterations { get; set; } = new();
}