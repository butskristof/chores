using Chores.Application.IntegrationTests.Common;
using Chores.Application.IntegrationTests.Common.Builders.Tags;
using Chores.Application.Modules.Tags;

namespace Chores.Application.IntegrationTests.Modules.Tags;

[Collection(nameof(ApplicationFixture))]
public sealed class GetTagsTests : ApplicationTestBase
{
    public GetTagsTests(ApplicationFixture application) : base(application)
    {
    }

    [Fact]
    public async Task ReturnsEmptyList()
    {
        var request = new GetTags.Request();
        var result = await Application.SendAsync(request);
        result.IsError.Should().BeFalse();
        result.Value.Tags.Should().BeEmpty();
    }

    [Fact]
    public async Task ReturnsDtos()
    {
        await Application.AddAsync(new TagBuilder().WithName("tag 1").Build());
        await Application.AddAsync(new TagBuilder().WithName("tag 2").Build());

        var request = new GetTags.Request();
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        result.Value.Tags
            .Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(t => t.Name == "tag 1")
            .And.ContainSingle(t => t.Name == "tag 2")
            .And.AllSatisfy(t => t.ChoresCount.Should().Be(0));
    }

    [Fact]
    public async Task ReturnsOnlyOwnedTags()
    {
        await Application.AddAsync(new TagBuilder().WithName("tag 1").Build());
        await Application.AddAsync(new TagBuilder().WithName("tag 2").Build());
        Application.SetUserId("other_user");
        await Application.AddAsync(new TagBuilder().WithName("tag 3").Build());

        var request = new GetTags.Request();
        var result = await Application.SendAsync(request);
        result.IsError.Should().BeFalse();
        result.Value.Tags
            .Should()
            .NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(t => t.Name == "tag 3");
    }

    [Fact]
    public async Task ReturnsChoresCount()
    {
        var tagId = new Guid("6DD146A2-6D36-45C8-8ABA-2BB2CD8D924F");
        await Application.AddAsync(new TagBuilder().WithId(tagId).Build());
        await Application.AddAsync(new ChoreBuilder().WithTags([tagId]).Build());
        
        var request = new GetTags.Request();
        var result = await Application.SendAsync(request);

        var tag = result.Value.Tags.SingleOrDefault();
        tag!.ChoresCount.Should().Be(1);
    }
}