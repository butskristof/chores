using ChoresPoc.Api.Data;

namespace ChoresPoc.Api.Tests.Data;

public static class TestDataSeeder
{
	public static void Seed(ChoreDbContext context)
	{
		context.Chores.Add(TestData.Chore1);

		context.SaveChanges();
	}
}