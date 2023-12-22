using Chores.Application.IntegrationTests.Common;
using Chores.Application.IntegrationTests.Common.Builders.Tags;
using Chores.Application.Modules.Chores;
using Chores.Domain.Models.Chores;
using ErrorOr;

namespace Chores.Application.IntegrationTests.Modules.Chores;

[Collection(nameof(ApplicationFixture))]
public sealed class UpdateChoreTagsTests : ApplicationTestBase
{
    public UpdateChoreTagsTests(ApplicationFixture application) : base(application)
    {
    }

    [Fact]
    public async Task InvalidChoreId_ReturnsNotFoundError()
    {
        var request = new UpdateChoreTags.Request(new Guid("D3A9AD8B-F47B-4F43-A470-3B275B74D500"), new List<Guid>());
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("non-existing ID should result in an error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("ChoreId");
    }

    [Fact]
    public async Task InvalidTagId_ReturnsNotFoundError()
    {
        var request = new UpdateChoreTags.Request(new Guid("D3A9AD8B-F47B-4F43-A470-3B275B74D500"), new List<Guid>());
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("non-existing ID should result in an error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("TagIds");
    }

    [Fact]
    public async Task InaccessibleChoreId_ReturnsNotFound()
    {
        var id = new Guid("CDA84CCC-912F-4B63-B109-7FF11A4A2761");
        await Application.AddAsync(new ChoreBuilder().WithId(id).Build());

        Application.SetUserId("other_user");
        var request = new UpdateChoreTags.Request(id, new List<Guid>());
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("non-existing ID should result in error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("ChoreId");

        var chore = await Application.FindAsync<Chore>(id);
        chore!.Tags.Should().BeEmpty();
    }

    [Fact]
    public async Task InaccessibleTagId_ReturnsNotFound()
    {
        var tagId = new Guid("609ACE69-BBD7-41EE-B8B0-03F23538C749");
        await Application.AddAsync(new TagBuilder().WithId(tagId).Build());

        Application.SetUserId("other_user");
        var choreId = new Guid("4EC92DD8-EE5F-40D3-ADFB-F988CBDA20DF");
        await Application.AddAsync(new ChoreBuilder().WithId(choreId).Build());

        var request = new UpdateChoreTags.Request(choreId, new[] { tagId });
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("non-existing ID should result in error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("TagId[0]");

        var chore = await Application.FindAsync<Chore>(choreId);
        chore!.Tags.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdatesTagIds()
    {
        var tagId = new Guid("BC51CF95-93B8-43A6-BCE8-8B2C022E91BA");
        await Application.AddAsync(new TagBuilder().WithId(tagId).Build());
        var choreId = new Guid("9EBC1BD0-DFB3-44DE-A6D9-D9CE6EB8B679");
        await Application.AddAsync(new ChoreBuilder().WithId(choreId).Build());

        var request = new UpdateChoreTags.Request(choreId, new[] { tagId });
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse("Tag should be added to Chore");
        result.Value.Should().Be(Result.Updated);

        var chore = await Application.FindAsync<Chore>(choreId);
        chore!.Tags.Should()
            .HaveCount(1)
            .And.ContainSingle(t => t.Id == tagId);
    }

    [Fact]
    public async Task ReplacesExistingTagId()
    {
        var tag1Id = new Guid("BC51CF95-93B8-43A6-BCE8-8B2C022E91BA");
        var tag1 = new TagBuilder().WithId(tag1Id).Build();
        await Application.AddAsync(tag1);
        var tag2Id = new Guid("6953DD2E-F562-44A6-BBD7-8FD971B4D3BD");
        await Application.AddAsync(new TagBuilder().WithId(tag2Id).Build());
        var choreId = new Guid("9EBC1BD0-DFB3-44DE-A6D9-D9CE6EB8B679");
        await Application.AddAsync(new ChoreBuilder().WithId(choreId).WithTags([tag1]).Build());

        var request = new UpdateChoreTags.Request(choreId, new[] { tag2Id });
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse("Tag should be added to Chore");
        result.Value.Should().Be(Result.Updated);

        var chore = await Application.FindAsync<Chore>(choreId);
        chore!.Tags.Should()
            .HaveCount(1)
            .And.ContainSingle(t => t.Id == tag2Id);
    }
}