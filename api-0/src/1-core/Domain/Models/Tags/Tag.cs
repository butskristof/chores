namespace Chores.Domain.Models.Tags;

public sealed class Tag
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
}