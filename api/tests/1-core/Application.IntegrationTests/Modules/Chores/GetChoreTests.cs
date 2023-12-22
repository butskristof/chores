using Chores.Application.IntegrationTests.Common;
using Chores.Application.IntegrationTests.Common.Builders.Tags;
using Chores.Application.Modules.Chores;
using ErrorOr;

namespace Chores.Application.IntegrationTests.Modules.Chores;

[Collection(nameof(ApplicationFixture))]
public sealed class GetChoreTests : ApplicationTestBase
{
    public GetChoreTests(ApplicationFixture application) : base(application)
    {
    }

    [Fact]
    public async Task InvalidId_ReturnsNotFoundError()
    {
        var request = new GetChore.Request(new Guid("CC66ABB2-262D-4597-A247-91EA05565159"));
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("non-existing ID should result in error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull();
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("Id");
    }

    [Fact]
    public async Task InaccessibleId_ReturnsNotFoundError()
    {
        var id = new Guid("EB81E443-3455-4C84-BD6C-31A2F28B3E7B");
        await Application.AddAsync(new ChoreBuilder().WithId(id).Build());
        Application.SetUserId("other_user");

        var request = new GetChore.Request(id);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("non-existing ID should result in error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull();
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("Id");
    }

    [Fact]
    public async Task ReturnsDto()
    {
        var id = new Guid("CC6EAAA0-DC01-4E23-B919-106BAC0F46C0");
        await Application.AddAsync(new ChoreBuilder()
            .WithId(id)
            .WithName("detailed chore")
            .WithInterval(3)
            .WithNotes("explicit notes")
            .Build());

        var request = new GetChore.Request(id);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        var dto = result.Value;
        dto.Should().NotBeNull();
        dto.Name.Should().Be("detailed chore");
        dto.Interval.Should().Be(3);
        dto.Notes.Should().Be("explicit notes");
    }

    [Fact]
    public async Task ReturnsDtoWithTagDtos()
    {
        var tagId = new Guid("78B2C708-A118-4DD4-AF83-4B3A7339E20C");
        await Application.AddAsync(new TagBuilder().WithId(tagId).WithName("this tag").Build());
        var choreId = new Guid("CE17BC94-15EE-4FD7-B47B-4A3CCEC47B61");
        await Application.AddAsync(new ChoreBuilder().WithId(choreId).WithTags([tagId]).Build());

        var request = new GetChore.Request(choreId);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        var tagDto = result.Value.Tags.SingleOrDefault();
        tagDto.Should().NotBeNull();
        tagDto!.Id.Should().Be(tagId);
        tagDto.Name.Should().Be("this tag");
    }
}