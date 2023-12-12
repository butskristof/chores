using Chores.Application.Common.Persistence;
using Chores.Application.IntegrationTests.Common.Database;
using Chores.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;

namespace Chores.Application.IntegrationTests.Common;

// this collection fixture sets up a database and builds a minimal, but representative, service collection
// so the application layer can be configured, with a scope factory as a result
// it also provides various utility methods to access the underlying database and authentication 
// so integration tests can be easily "arranged" before "acting" by sending requests into the mediator

[CollectionDefinition(nameof(TestFixture))]
public class TestFixtureCollection : ICollectionFixture<TestFixture>;

public sealed class TestFixture : IAsyncLifetime
{
    // these explicit nulls are unfortunate, but since we're using IAsyncLifetime and setting them there
    // it's somewhat acceptable
    private ITestDatabase _database = null!;
    private IServiceScopeFactory _scopeFactory = null!;
    private readonly FakeTimeProvider _timeProvider = new();

    public async Task InitializeAsync()
    {
        // acquire a database (we assume this to be set up)
        _database = await TestDatabaseFactory.CreateAsync();

        // build a service collection
        var services = new ServiceCollection();
        // start by adding the application layer
        services
            .AddApplication();

        // add in required ports & adapters by replacing them with fakes
        // infrastructure
        services
            .AddLogging()
            .AddSingleton<TimeProvider>(_timeProvider);

        // persistence
        services
            .AddDbContext<AppDbContext>(options => { options.UseSqlServer(_database.GetConnection()); })
            .AddScoped<IAppDbContext, AppDbContext>();

        // build a scope factory from the service collection
        var provider = services.BuildServiceProvider();
        _scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
    }

    public async Task ResetStateAsync()
    {
        try
        {
            await _database.ResetAsync();
        }
        catch (Exception)
        {
        }

        // TODO userId
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        return await sender.Send(request);
    }

    public async Task SendAsync(IBaseRequest request)
    {
        using var scope = _scopeFactory.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        await sender.Send(request);
    }

    public async Task DisposeAsync()
    {
        await _database.DisposeAsync();
    }
}