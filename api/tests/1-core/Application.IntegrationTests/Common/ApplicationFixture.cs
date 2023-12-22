using System.Linq.Expressions;
using Chores.Application.Common.Authentication;
using Chores.Application.IntegrationTests.Common.Database;
using Chores.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;

namespace Chores.Application.IntegrationTests.Common;

// this collection fixture sets up a database and builds a minimal, but representative, service collection
// so the application layer can be configured, with a scope factory as a result
// it also provides various utility methods to access the underlying database and authentication 
// so integration tests can be easily "arranged" before "acting" by sending requests into the mediator

[CollectionDefinition(nameof(ApplicationFixture))]
public class TestFixtureCollection : ICollectionFixture<ApplicationFixture>;

public sealed class ApplicationFixture : IAsyncLifetime
{
    // these explicit nulls are unfortunate, but since we're using IAsyncLifetime and setting them there
    // it's somewhat acceptable
    private ITestDatabase _database = null!;
    private IServiceScopeFactory _scopeFactory = null!;
    private FakeTimeProvider _timeProvider = new();
    private string? _userId;

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
            .AddScoped<TimeProvider>(_ => _timeProvider);

        // persistence
        services
            .AddPersistence(null, _database.GetConnection());

        // authentication
        var authenticationInfo = Substitute.For<IAuthenticationInfo>();
        authenticationInfo
            .UserId
            .Returns(_ => GetUserId());
        services
            .AddSingleton(authenticationInfo);

        // build a scope factory from the service collection
        var provider = services.BuildServiceProvider();
        _scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
    }

    public async Task ResetStateAsync(string? userId = null)
    {
        await _database.ResetAsync();
        _timeProvider = new FakeTimeProvider();
        SetUserId(userId);
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

    public async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.FindAsync<TEntity>(keyValues);
    }

    public async Task<TEntity?> FindAsync<TEntity>(
        Expression<Func<TEntity, bool>> identifier,
        params Expression<Func<TEntity, object>>[] includes)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var query = context.Set<TEntity>().AsQueryable();
        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.SingleOrDefaultAsync(identifier);
    }

    public async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Add(entity);
        await context.SaveChangesAsync();
    }

    public async Task<int> CountAsync<TEntity>()
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        return await context.Set<TEntity>().CountAsync();
    }

    public void SetDateTime(DateTimeOffset dateTime)
        => _timeProvider.SetUtcNow(dateTime);

    public string? GetUserId() => _userId;
    public void SetUserId(string? userId) => _userId = userId;

    public async Task DisposeAsync()
    {
        await _database.DisposeAsync();
    }
}