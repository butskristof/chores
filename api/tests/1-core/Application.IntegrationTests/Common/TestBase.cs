namespace Chores.Application.IntegrationTests.Common;

// this base class is intended to be inherited by nearly every integration test class: it adds the inherited class 
// to the collection, accepts and sets up the collection fixture and resets state for each test

[Collection(nameof(TestFixture))]
public abstract class TestBase : IAsyncLifetime
{
    public TestFixture Fixture { get; }

    protected TestBase(TestFixture fixture)
    {
        Fixture = fixture;
    }

    public async Task InitializeAsync()
    {
        await Fixture.ResetStateAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}