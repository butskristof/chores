namespace Chores.Application.IntegrationTests.Common.Database;

// this factory initialises an ITestDatabase instance for use in the test fixture 
// various database providers are possible through configuration

internal static class TestDatabaseFactory
{
    internal static async Task<ITestDatabase> CreateAsync()
    {
        // TODO add SQL Server & Sqlite w/ configuration
        ITestDatabase database = new TestcontainersTestDatabase();
        await database.InitializeAsync();
        return database;
    }
}