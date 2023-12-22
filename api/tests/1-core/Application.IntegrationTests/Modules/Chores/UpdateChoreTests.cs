using Chores.Application.IntegrationTests.Common;
using Chores.Application.IntegrationTests.Common.Builders.Tags;
using Chores.Application.Modules.Chores;
using Chores.Domain.Models.Chores;
using ErrorOr;

namespace Chores.Application.IntegrationTests.Modules.Chores;

[Collection(nameof(ApplicationFixture))]
public sealed class UpdateChoreTests : ApplicationTestBase
{
    public UpdateChoreTests(ApplicationFixture application) : base(application)
    {
    }

    [Fact]
    public async Task InvalidRequest_ReturnsValidationError()
    {
        var request = new UpdateChore.Request(Guid.Empty, string.Empty, 1);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("validation failure should be reported as error");

        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.Validation);
        error.Code.Should().Be("Name");
        error.Description.Should().Be("Required");
    }

    [Fact]
    public async Task InvalidId_ReturnsNotFoundError()
    {
        var request = new UpdateChore.Request(new Guid("2707AFBC-BBFB-4722-B869-433120AF7117"), "do something", 1);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("non-existing ID should result in an error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("Id");
    }

    [Fact]
    public async Task InaccessibleId_ReturnsNotFoundErrorAndDoesNotUpdateChore()
    {
        var id = new Guid("3307A6DC-82F3-4BD8-90C6-7054F715D5BD");
        var created = new DateTimeOffset(2023, 12, 18, 20, 12, 11, TimeSpan.Zero);
        Application.SetDateTime(created);
        await Application.AddAsync(new ChoreBuilder().WithId(id).WithName("inaccessible").Build());

        var modified = new DateTimeOffset(2023, 12, 18, 20, 13, 55, TimeSpan.Zero);
        Application.SetDateTime(modified);
        Application.SetUserId("other_user");
        var request = new UpdateChore.Request(id, "do something", 1);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("non-owned chore should not be accessible");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("Id");

        var chore = await Application.FindAsync<Chore>(id);
        chore.Should().NotBeNull();
        chore!.Name.Should().Be("inaccessible");
        chore.LastModifiedOn.Should().Be(created);
        chore.LastModifiedBy.Should().Be(TestConstants.DefaultUserId);
    }

    [Fact]
    public async Task UpdatesChore()
    {
        var id = new Guid("69E593CF-C8F6-4468-B69A-6E2C317413E4");
        var created = new DateTimeOffset(2023, 12, 18, 20, 15, 22, TimeSpan.Zero);
        Application.SetDateTime(created);
        await Application.AddAsync(new ChoreBuilder().WithId(id).Build());

        var modified = new DateTimeOffset(2023, 12, 18, 20, 16, 5, TimeSpan.Zero);
        Application.SetDateTime(modified);
        var request = new UpdateChore.Request(id, "new name", 5);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Updated);

        var expectedChore = new Chore
        {
            Id = id,
            Name = "new name",
            Interval = 5,
            CreatedBy = TestConstants.DefaultUserId,
            CreatedOn = created,
            LastModifiedBy = TestConstants.DefaultUserId,
            LastModifiedOn = modified,
        };
        var chore = await Application.FindAsync<Chore>(id);
        chore.Should().BeEquivalentTo(expectedChore);
    }
}