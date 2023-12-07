namespace Chores.Domain.Models.Chores;

public sealed class Chore
{
    public Guid Id { get; set; }
    
    public required string Title { get; set; }
    public int Frequency { get; set; }

    public string? Notes { get; set; }

    public ICollection<ChoreIteration> Iterations { get; set; } = new List<ChoreIteration>();
    public ICollection<ChoreTag> Tags { get; set; } = new List<ChoreTag>();
}