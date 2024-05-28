using System.Data.Common;
using Chores.Application.Common.Authentication;
using Chores.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NSubstitute;
using Respawn;
using Respawn.Graph;
using Testcontainers.PostgreSql;

namespace Chores.Application.IntegrationTests.Common.Database;

// this implementation uses the Testcontainers library to spin up a SQL Server Docker container,
// prepare the database and dispose of it afterwards
// it assures a self-contained process against a production-grade database, but can suffer from 
// longer startup times since the container has to be prepared before each test run

internal sealed class TestcontainersTestDatabase : ITestDatabase
{
    private readonly PostgreSqlContainer _container;
    private DbConnection _connection = null!;
    private string _connectionString = null!;
    private Respawner _respawner = null!;

    public TestcontainersTestDatabase()
    {
        _container = new PostgreSqlBuilder()
            .WithAutoRemove(true)
            .WithDatabase("chores")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        _connectionString = _container.GetConnectionString();
        _connection = new NpgsqlConnection(_connectionString);
        await _connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_connectionString)
            .Options;
        var context = new AppDbContext(options, Substitute.For<IAuthenticationInfo>());
        context.Database.Migrate();
        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            TablesToIgnore = new Table[] { "__EFMigrationsHistory" },
            DbAdapter = DbAdapter.Postgres,
        });
    }

    public DbConnection GetConnection() => _connection;

    public Task ResetAsync() => _respawner.ResetAsync(_connection);

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
        await _container.DisposeAsync();
    }
}