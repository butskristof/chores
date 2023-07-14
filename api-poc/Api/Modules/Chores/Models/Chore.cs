namespace ChoresPoc.Api.Modules.Chores.Models;

public class Chore
{
	public Guid Id { get; set; }
	public required string Title { get; set; }
	public int Interval { get; set; }
}