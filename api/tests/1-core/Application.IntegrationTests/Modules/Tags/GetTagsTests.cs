using Chores.Application.IntegrationTests.Common;
using Chores.Application.Modules.Tags;
using Chores.Domain.Models;

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
        {
            await Application.AddAsync(new Tag { Name = "tag 1" });
            await Application.AddAsync(new Tag { Name = "tag 2" });
        }
        var request = new GetTags.Request();
        var result = await Application.SendAsync(request);
        result.IsError.Should().BeFalse();
        result.Value.Tags
            .Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(t => t.Name == "tag 1")
            .And.ContainSingle(t => t.Name == "tag 2");
    }
}