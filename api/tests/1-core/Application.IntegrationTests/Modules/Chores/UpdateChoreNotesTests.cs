using Chores.Application.IntegrationTests.Common;
using Chores.Application.IntegrationTests.Common.Builders.Tags;
using Chores.Application.Modules.Chores;
using Chores.Domain.Models.Chores;
using ErrorOr;

namespace Chores.Application.IntegrationTests.Modules.Chores;

[Collection(nameof(ApplicationFixture))]
public sealed class UpdateChoreNotesTests : ApplicationTestBase
{
    public UpdateChoreNotesTests(ApplicationFixture application) : base(application)
    {
    }

    [Fact]
    public async Task InvalidId_ReturnsNotFoundError()
    {
        var request = new UpdateChoreNotes.Request(new Guid("82FC3A38-467B-4243-AB49-AA4DBC7FEB4F"), null);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("non-existing ID should result in error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("ChoreId");
    }

    [Fact]
    public async Task InaccessibleId_ReturnsNotFoundErrorAndDoesNotUpdateChore()
    {
        var id = new Guid("95696099-FDDD-499A-9FDC-78DCABCF252F");
        var created = new DateTimeOffset(2023, 12, 18, 21, 0, 55, TimeSpan.Zero);
        Application.SetDateTime(created);
        await Application.AddAsync(new ChoreBuilder().WithId(id).Build());

        var modified = new DateTimeOffset(2023, 12, 18, 21, 1, 20, TimeSpan.Zero);
        Application.SetDateTime(modified);
        Application.SetUserId("other_user");
        var request = new UpdateChoreNotes.Request(id, "some content");
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("non-existing ID should result in error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("ChoreId");
    }

    [Theory]
    [InlineData("notes content")]
    [InlineData(null)]
    public async Task UpdatesNotes(string? input)
    {
        var id = new Guid("96AFF434-F605-4111-94C9-BD162E2FB060");
        var created = new DateTimeOffset(2023, 12, 18, 21, 2, 33, TimeSpan.Zero);
        Application.SetDateTime(created);
        await Application.AddAsync(new ChoreBuilder().WithId(id).WithNotes("some notes").Build());

        var modified = new DateTimeOffset(2023, 12, 18, 21, 3, 11, TimeSpan.Zero);
        Application.SetDateTime(modified);
        var request = new UpdateChoreNotes.Request(id, input);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse("should indicate success");
        result.Value.Should().Be(Result.Updated);

        var chore = await Application.FindAsync<Chore>(id);
        chore!.Notes.Should().Be(input);
        chore.LastModifiedOn.Should().Be(modified);
        chore.LastModifiedBy.Should().Be(TestConstants.DefaultUserId);
    }

    [Fact]
    public async Task LongerThanDefaultAllowedStringLength_UpdatesNotes()
    {
        var id = new Guid("96AFF434-F605-4111-94C9-BD162E2FB060");
        var created = new DateTimeOffset(2023, 12, 18, 21, 2, 33, TimeSpan.Zero);
        Application.SetDateTime(created);
        await Application.AddAsync(new ChoreBuilder().WithId(id).Build());

        var modified = new DateTimeOffset(2023, 12, 18, 21, 3, 11, TimeSpan.Zero);
        Application.SetDateTime(modified);
        var notes = new string('*', 513);
        var request = new UpdateChoreNotes.Request(id, notes);
        var result = await Application.SendAsync(request);


        result.IsError.Should().BeFalse("should indicate success");
        result.Value.Should().Be(Result.Updated);

        var chore = await Application.FindAsync<Chore>(id);
        chore!.Notes.Should().Be(notes);
        chore.LastModifiedOn.Should().Be(modified);
        chore.LastModifiedBy.Should().Be(TestConstants.DefaultUserId);
    }
}