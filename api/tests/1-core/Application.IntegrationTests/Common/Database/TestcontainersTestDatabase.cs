using System.Data.Common;
using Chores.Application.Common.Authentication;
using Chores.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Respawn;
using Respawn.Graph;
using Testcontainers.MsSql;

namespace Chores.Application.IntegrationTests.Common.Database;

// this implementation uses the Testcontainers library to spin up a SQL Server Docker container,
// prepare the database and dispose of it afterwards
// it assures a self-contained process against a production-grade database, but can suffer from 
// longer startup times since the container has to be prepared before each test run

internal sealed class TestcontainersTestDatabase : ITestDatabase
{
    private readonly MsSqlContainer _container;
    private DbConnection _connection = null!;
    private string _connectionString = null!;
    private Respawner _respawner = null!;

    public TestcontainersTestDatabase()
    {
        _container = new MsSqlBuilder()
            .WithAutoRemove(true)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        _connectionString = _container.GetConnectionString();
        _connection = new SqlConnection(_connectionString);
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(_connectionString)
            .Options;
        var context = new AppDbContext(options, Substitute.For<IAuthenticationInfo>());
        context.Database.Migrate();
        _respawner = await Respawner.CreateAsync(_connectionString, new RespawnerOptions
        {
            TablesToIgnore = new Table[] { "__EFMigrationsHistory" },
        });
    }

    public DbConnection GetConnection() => _connection;

    public Task ResetAsync() => _respawner.ResetAsync(_connectionString);

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
        await _container.DisposeAsync();
    }
}