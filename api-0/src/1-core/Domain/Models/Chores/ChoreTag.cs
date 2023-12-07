namespace Chores.Domain.Models.Chores;

public sealed class ChoreTag
{
    public Guid ChoreId { get; set; }
    public Guid TagId { get; set; }
}