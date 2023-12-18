using Chores.Application.IntegrationTests.Common;
using Chores.Application.IntegrationTests.Common.Builders.Tags;
using Chores.Application.Modules.Chores;

namespace Chores.Application.IntegrationTests.Modules.Chores;

[Collection(nameof(ApplicationFixture))]
public sealed class GetChoresTests : ApplicationTestBase
{
    public GetChoresTests(ApplicationFixture application) : base(application)
    {
    }

    [Fact]
    public async Task ReturnsEmptyList()
    {
        var request = new GetChores.Request();
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        result.Value.Chores.Should().BeEmpty();
    }

    [Fact]
    public async Task ReturnsDtos()
    {
        await Application.AddAsync(new ChoreBuilder().WithName("chore 1").Build());
        await Application.AddAsync(new ChoreBuilder().WithName("chore 2").Build());

        var request = new GetChores.Request();
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        result.Value.Chores
            .Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(c => c.Name == "chore 1")
            .And.ContainSingle(c => c.Name == "chore 2");
    }

    [Fact]
    public async Task ReturnsOnlyOwnedChores()
    {
        await Application.AddAsync(new ChoreBuilder().WithName("hidden").Build());
        Application.SetUserId("other_user");
        await Application.AddAsync(new ChoreBuilder().WithName("visible").Build());

        var request = new GetChores.Request();
        var result = await Application.SendAsync(request);

        result.Value.Chores
            .Should()
            .HaveCount(1)
            .And.ContainSingle(c => c.Name == "visible");
    }
}