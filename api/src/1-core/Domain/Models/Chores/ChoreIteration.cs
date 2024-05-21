using Chores.Domain.Common;

namespace Chores.Domain.Models.Chores;

public sealed class ChoreIteration : BaseAuditableEntity
{
    public Guid ChoreId { get; set; }
    
    public DateOnly Date { get; set; }
    public string? Notes { get; set; }
}