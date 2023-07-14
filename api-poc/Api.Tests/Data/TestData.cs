using ChoresPoc.Api.Modules.Chores.Models;

namespace ChoresPoc.Api.Tests.Data;

public static class TestData
{
	public static readonly Guid Chore1Id = Guid.Parse("2E26D0F0-2AB2-46C9-8C93-2A7A97A6174A");
	public static readonly Chore Chore1 = new()
	{
		Id = Chore1Id,
		Title = "chore 1", 
		Interval = 1,
	};
}