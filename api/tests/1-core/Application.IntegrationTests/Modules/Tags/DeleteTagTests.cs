using Chores.Application.IntegrationTests.Common;
using Chores.Application.Modules.Tags;
using Chores.Domain.Models;
using ErrorOr;

namespace Chores.Application.IntegrationTests.Modules.Tags;

[Collection(nameof(ApplicationFixture))]
public sealed class DeleteTagTests : ApplicationTestBase
{
    public DeleteTagTests(ApplicationFixture application) : base(application)
    {
    }

    [Fact]
    public async Task InvalidId_ReturnsNotFoundError()
    {
        var request = new DeleteTag.Request(new Guid("F142362E-E424-469D-B495-3FD360A88CE5"));
        var result = await Application.SendAsync(request);
        result.IsError.Should().BeTrue();
        result.ErrorsOrEmptyList.Single().Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task DeletesTask()
    {
        var id = new Guid("5B6035C2-9FB6-433D-B315-C6AE1384274B");
        {
            var tag = new Tag { Id = id, Name = string.Empty};
            await Application.AddAsync(tag);
        }
        var request = new DeleteTag.Request(id);
        var result = await Application.SendAsync(request);
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Deleted);

        (await Application.FindAsync<Tag>(id))
            .Should()
            .BeNull();
    }
}