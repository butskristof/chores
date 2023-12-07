namespace Chores.Domain.Models.Chores;

public sealed class ChoreIteration
{
    public Guid Id { get; set; }
    public Guid ChoreId { get; set; }
    
    public DateTimeOffset Timestamp { get; set; }
    public string? Notes { get; set; }
}