using Chores.Domain.Common;

namespace Chores.Domain.Models;

public sealed class Tag : BaseAuditableEntity
{
    public required string Name { get; set; }
}