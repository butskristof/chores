using Chores.Application.IntegrationTests.Common;

namespace Chores.Application.IntegrationTests.Modules.Chores;

[Collection(nameof(ApplicationFixture))]
public sealed class CreateChoreTests : ApplicationTestBase
{
    public CreateChoreTests(ApplicationFixture application) : base(application)
    {
    }

    [Fact]
    public async Task CreatesChore()
    {
        await Task.Delay(1);
        Assert.True(false);
    }
}