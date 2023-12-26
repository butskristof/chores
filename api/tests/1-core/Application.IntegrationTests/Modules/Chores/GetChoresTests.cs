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
        await Application.AddAsync(new ChoreBuilder().WithName("chore 1").WithInterval(1).Build());
        await Application.AddAsync(new ChoreBuilder().WithName("chore 2").WithInterval(2).Build());

        var request = new GetChores.Request();
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        result.Value.Chores
            .Should()
            .HaveCount(2)
            .And
            .SatisfyRespectively(
                c => c.Should().BeEquivalentTo(new { Name = "chore 1", Interval = 1 }),
                c => c.Should().BeEquivalentTo(new { Name = "chore 2", Interval = 2 })
            );
    }

    [Fact]
    public async Task ReturnsChoreDtosWithTagDtos()
    {
        var tagId = new Guid("60C61DC7-EAC4-49F6-AF55-9F9395851514");
        await Application.AddAsync(new TagBuilder().WithId(tagId).WithName("some tag").Build());
        await Application.AddAsync(new ChoreBuilder().WithTags([tagId]).Build());

        var request = new GetChores.Request();
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        var choreDto = result.Value.Chores.SingleOrDefault();
        choreDto.Should().NotBeNull();
        choreDto!.Tags
            .Should().HaveCount(1)
            .And.ContainSingle(t => t.Id == tagId && t.Name == "some tag");
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

    [Fact]
    public async Task ReturnsNullIfNoIterations()
    {
        var choreId = new Guid("19E758A9-3DA9-444A-BC7B-D00E675749BA");
        await Application.AddAsync(new ChoreBuilder().WithId(choreId).Build());

        var request = new GetChores.Request();
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        var chore = result.Value.Chores.SingleOrDefault();
        chore.Should().NotBeNull();
        chore!.LastIteration.Should().BeNull();
    }

    [Fact]
    public async Task ReturnsLastIteration()
    {
        Application.SetDateTime(new DateTimeOffset(2023, 12, 24, 15, 2, 33, TimeSpan.Zero));
        var choreId = new Guid("19E758A9-3DA9-444A-BC7B-D00E675749BA");
        await Application.AddAsync(new ChoreBuilder()
            .WithId(choreId)
            .WithIterations([
                new ChoreIterationBuilder().WithDate(new DateTimeOffset(2023, 12, 15, 0, 0, 0, TimeSpan.Zero)),
                new ChoreIterationBuilder().WithDate(new DateTimeOffset(2023, 12, 24, 0, 0, 0, TimeSpan.Zero)),
                new ChoreIterationBuilder().WithDate(new DateTimeOffset(2023, 11, 24, 0, 0, 0, TimeSpan.Zero)),
            ])
            .Build());

        var request = new GetChores.Request();
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        var chore = result.Value.Chores.SingleOrDefault();
        chore.Should().NotBeNull();
        chore!.LastIteration.Should().Be(new DateTimeOffset(2023, 12, 24, 0, 0, 0, TimeSpan.Zero));
    }
}