using System.Data.Common;

namespace Chores.Application.IntegrationTests.Common.Database;

// this interface defines all methods to set up a database to use with the EF Core DbContext
// it allows to implement different providers such as SQL Server, Testcontainers, in-memory SQLite, ...

internal interface ITestDatabase
{
    Task InitializeAsync();
    DbConnection GetConnection();
    Task ResetAsync();
    Task DisposeAsync();
}