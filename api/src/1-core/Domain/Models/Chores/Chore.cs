using Chores.Domain.Common;

namespace Chores.Domain.Models.Chores;

public sealed class Chore : BaseAuditableEntity
{
    public required string Name { get; set; }
    public required int Interval { get; set; }
}