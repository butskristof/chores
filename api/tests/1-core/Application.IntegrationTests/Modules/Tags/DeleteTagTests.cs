using Chores.Application.IntegrationTests.Common;
using Chores.Application.IntegrationTests.Common.Builders.Tags;
using Chores.Application.Modules.Tags;
using Chores.Domain.Models.Tags;
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

        result.IsError.Should().BeTrue("random ID should not be found");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("Id");
    }

    [Fact]
    public async Task InaccessibleId_ReturnsNotFoundErrorAndDoesNotDeleteTag()
    {
        var id = new Guid("8DA0E36D-898E-4682-8084-099209EF1173");
        await Application.AddAsync(new TagBuilder().WithId(id).Build());

        Application.SetUserId("other_user");
        var request = new DeleteTag.Request(id);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("non-owned tag should be inaccessible");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("Id");

        var tag = await Application.FindAsync<Tag>(id);
        tag.Should().NotBeNull();
    }

    [Fact]
    public async Task ChoresAttached_ReturnsErrorAndDoesNotDeleteTag()
    {
        var tagId = new Guid("FDEF93C4-D0D0-4647-ABF3-31EC846E2C66");
        await Application.AddAsync(new TagBuilder().WithId(tagId).Build());
        var choreId = new Guid("464B97F6-5C61-47E8-A279-4B10CD8990AA");
        await Application.AddAsync(new ChoreBuilder().WithId(choreId).WithTags([tagId]).Build());

        var request = new DeleteTag.Request(tagId);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("tag referenced by chore should not be deleted");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.Validation);
        error.Code.Should().Be("Chores");

        var tag = await Application.FindAsync<Tag>(tagId);
        tag.Should().NotBeNull();
    }

    [Fact]
    public async Task DeletesTag()
    {
        var id = new Guid("5B6035C2-9FB6-433D-B315-C6AE1384274B");
        await Application.AddAsync(new TagBuilder().WithId(id).Build());
        var request = new DeleteTag.Request(id);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Deleted);

        (await Application.FindAsync<Tag>(id))
            .Should()
            .BeNull();
    }
}