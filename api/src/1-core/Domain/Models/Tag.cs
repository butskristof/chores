namespace Chores.Domain.Models;

public sealed class Tag
{
    public Guid Id { get; init; }
    public required string Name { get; set; }
}