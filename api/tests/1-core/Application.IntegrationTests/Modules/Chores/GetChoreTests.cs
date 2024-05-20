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
        dto.Id.Should().Be(id);
        dto.Name.Should().Be("detailed chore");
        dto.Interval.Should().Be(3);
        dto.Notes.Should().Be("explicit notes");
    }

    [Fact]
    public async Task ReturnsDtoWithTagDtos()
    {
        var tagId = new Guid("78B2C708-A118-4DD4-AF83-4B3A7339E20C");
        await Application.AddAsync(new TagBuilder()
            .WithId(tagId)
            .WithName("this tag")
            .WithColor("#000000")
            .WithIcon("pi pi-check")
            .Build());
        var choreId = new Guid("CE17BC94-15EE-4FD7-B47B-4A3CCEC47B61");
        await Application.AddAsync(new ChoreBuilder().WithId(choreId).WithTags([tagId]).Build());

        var request = new GetChore.Request(choreId);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        var tagDto = result.Value.Tags.SingleOrDefault();
        var expectedTagDto = new GetChore.TagDto(tagId, "this tag", "#000000", "pi pi-check");
        tagDto.Should().BeEquivalentTo(expectedTagDto);
    }

    [Fact]
    public async Task ReturnsDtoWithIterationDtos()
    {
        Application.SetDateTime(new DateTimeOffset(2023, 12, 24, 11, 40, 23, TimeSpan.Zero));
        var choreId = new Guid("322482D7-B0D9-4B16-BB61-8F790F784099");
        var iterationId = new Guid("A60498A5-5279-442E-AE53-209F389C7441");
        await Application.AddAsync(new ChoreBuilder()
            .WithId(choreId)
            .WithIterations([
                new ChoreIterationBuilder()
                    .WithId(iterationId)
                    .WithNotes("some notes")
                    .WithDate(new DateOnly(2023, 12, 24))
                    .Build()
            ])
            .Build());

        var request = new GetChore.Request(choreId);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        var iterationDto = result.Value.Iterations.SingleOrDefault();
        iterationDto.Should().NotBeNull();
        iterationDto.Should()
            .BeEquivalentTo(new GetChore.IterationDto(iterationId,
                new DateOnly(2023, 12, 24), "some notes"));
    }

    [Fact]
    public async Task ReturnsDtoWithIterationsInDescendingOrder()
    {
        Application.SetDateTime(new DateTimeOffset(2023, 12, 24, 14, 55, 54, TimeSpan.Zero));
        var choreId = new Guid("C83647F8-18AD-4EA5-8191-924EE4306ECE");
        await Application.AddAsync(new ChoreBuilder()
            .WithId(choreId)
            .WithIterations([
                new ChoreIterationBuilder()
                    .WithDate(new DateOnly(2023, 12, 24)),
                new ChoreIterationBuilder()
                    .WithDate(new DateOnly(2023, 12, 23)),
                new ChoreIterationBuilder()
                    .WithDate(new DateOnly(2023, 11, 23))
            ])
            .Build());

        var request = new GetChore.Request(choreId);
        var result = await Application.SendAsync(request);

        result.Value.Iterations
            .Should()
            .BeInDescendingOrder(i => i.Date);
    }
}