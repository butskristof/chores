namespace Chores.Application.IntegrationTests.Common;

// this base class is intended to be inherited by nearly every integration test class: it adds the inherited class 
// to the collection, accepts and sets up the collection fixture and resets state for each test

[Collection(nameof(ApplicationFixture))]
public abstract class ApplicationTestBase : IAsyncLifetime
{
    protected ApplicationFixture Application { get; }

    protected ApplicationTestBase(ApplicationFixture application)
    {
        Application = application;
    }

    public async Task InitializeAsync()
    {
        // since this functions as basically an async constructor, it will be executed before each test
        // hence, we reset the state before every single test
        await Application.ResetStateAsync(userId: TestConstants.DefaultUserId);
    }

    public Task DisposeAsync() => Task.CompletedTask;
}