using Chores.Application.IntegrationTests.Common;
using Chores.Application.IntegrationTests.Common.Builders.Tags;
using Chores.Application.Modules.Tags;
using Chores.Domain.Models.Tags;
using ErrorOr;

namespace Chores.Application.IntegrationTests.Modules.Tags;

[Collection(nameof(ApplicationFixture))]
public sealed class UpdateTagTests : ApplicationTestBase
{
    public UpdateTagTests(ApplicationFixture application) : base(application)
    {
    }

    [Fact]
    public async Task InvalidRequest_ReturnsValidationError()
    {
        var request = new UpdateTag.Request(Guid.Empty, string.Empty);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("validation failure should be reported as error");

        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("errors should only contain one validation error");
        error.Type.Should().Be(ErrorType.Validation);
        error.Code.Should().Be("Name");
        error.Description.Should().Be("Required");
    }

    [Fact]
    public async Task InvalidId_ReturnsNotFoundError()
    {
        var request = new UpdateTag.Request(new Guid("F142362E-E424-469D-B495-3FD360A88CE5"), "valid name");
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("random ID should not be found");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("Id");
    }

    [Fact]
    public async Task DuplicateName_ReturnsConflictError()
    {
        var id = new Guid("F9123C25-0597-481D-BA97-AA1B9BC884A4");
        await Application.AddAsync(new TagBuilder().WithName("some name").Build());
        await Application.AddAsync(new TagBuilder().WithId(id).WithName("other name").Build());

        var request = new UpdateTag.Request(id, "some name");
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("conflict should be an error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.Conflict);
        error.Code.Should().Be("Name");
    }

    [Fact]
    public async Task InaccessibleId_ReturnsNotFoundErrorAndDoesNotUpdateTag()
    {
        var id = new Guid("8DA0E36D-898E-4682-8084-099209EF1173");
        var created = new DateTimeOffset(2023, 12, 13, 14, 32, 0, TimeSpan.Zero);
        Application.SetDateTime(created);
        await Application.AddAsync(new TagBuilder().WithId(id).WithName("inaccessible").Build());

        var modified = new DateTimeOffset(2023, 12, 13, 14, 32, 0, TimeSpan.Zero);
        Application.SetDateTime(modified);
        Application.SetUserId("other_user");
        var request = new UpdateTag.Request(id, "valid name");
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("non-owned tag should not be accessible");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("Id");

        var tag = await Application.FindAsync<Tag>(id);
        tag!.Name.Should().Be("inaccessible");
        tag.LastModifiedOn.Should().Be(created);
        tag.LastModifiedBy.Should().Be(TestConstants.DefaultUserId);
    }

    [Fact]
    public async Task UpdatesTag()
    {
        var id = new Guid("A6411F21-A841-450D-BD52-D41A2F36D8B8");
        var created = new DateTimeOffset(2023, 12, 13, 13, 22, 33, TimeSpan.Zero);
        var modified = new DateTimeOffset(2023, 12, 13, 13, 23, 34, TimeSpan.Zero);
        {
            Application.SetDateTime(created);
            await Application.AddAsync(new TagBuilder().WithId(id).Build());
        }

        Application.SetDateTime(modified);
        var request = new UpdateTag.Request(id, "some name");
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Updated);

        var expectedTag = new Tag
        {
            Id = id,
            Name = "some name",
            CreatedBy = TestConstants.DefaultUserId,
            CreatedOn = created,
            LastModifiedBy = TestConstants.DefaultUserId,
            LastModifiedOn = modified,
        };
        var tag = await Application.FindAsync<Tag>(id);
        tag.Should().BeEquivalentTo(expectedTag);
    }

    [Fact]
    public async Task DuplicateNameOfOtherUser_UpdatesTag()
    {
        await Application.AddAsync(new TagBuilder().WithName("desired name").Build());
        Application.SetUserId("other_user");
        var id = new Guid("449D9B52-5165-4692-8EDF-58BF9CC77969");
        await Application.AddAsync(new TagBuilder().WithId(id).Build());

        var request = new UpdateTag.Request(id, "desired name");
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();

        var tag = await Application.FindAsync<Tag>(id);
        tag!.Name.Should().Be("desired name");
    }
}