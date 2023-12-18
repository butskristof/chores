using Chores.Application.IntegrationTests.Common;
using Chores.Application.IntegrationTests.Common.Builders.Tags;
using Chores.Application.Modules.Chores;
using Chores.Application.Modules.Tags;
using Chores.Domain.Models.Chores;
using ErrorOr;

namespace Chores.Application.IntegrationTests.Modules.Chores;

[Collection(nameof(ApplicationFixture))]
public sealed class DeleteChoreTests : ApplicationTestBase
{
    public DeleteChoreTests(ApplicationFixture application) : base(application)
    {
    }

    [Fact]
    public async Task InvalidId_ReturnsNotFoundError()
    {
        var request = new DeleteTag.Request(new Guid("71A86D49-F967-4FE0-9000-F6450475D57D"));

        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("random ID should not be found");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("Id");
    }

    [Fact]
    public async Task InaccessibleId_ShouldReturnNotFoundAndNotDeleteChore()
    {
        var id = new Guid("2B125217-1522-4ED8-B18D-E8B6FCC86AF5");
        await Application.AddAsync(new ChoreBuilder().WithId(id).Build());

        Application.SetUserId("other_user");
        var request = new DeleteChore.Request(id);
        var result = await Application.SendAsync(request);
        
        result.IsError.Should().BeTrue("non-owned chore should be inaccessible");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("Id");

        var chore = await Application.FindAsync<Chore>(id);
        chore.Should().NotBeNull();
    }

    [Fact]
    public async Task DeletesChore()
    {
        var id = new Guid("0F76D8C3-EF25-4530-982F-E0C0A9C5EA69");
        await Application.AddAsync(new ChoreBuilder().WithId(id).Build());

        var request = new DeleteChore.Request(id);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse("should be returned as successful operation");
        result.Value.Should().Be(Result.Deleted);

        var chore = await Application.FindAsync<Chore>(id);
        chore.Should().BeNull();
    }

    // TODO verify iteration cascade
}