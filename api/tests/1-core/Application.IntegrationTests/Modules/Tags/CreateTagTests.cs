using Chores.Application.IntegrationTests.Common;
using Chores.Application.IntegrationTests.Common.Builders.Tags;
using Chores.Application.Modules.Tags;
using Chores.Domain.Models.Tags;
using ErrorOr;

namespace Chores.Application.IntegrationTests.Modules.Tags;

[Collection(nameof(ApplicationFixture))]
public sealed class CreateTagTests : ApplicationTestBase
{
    public CreateTagTests(ApplicationFixture application) : base(application)
    {
    }

    [Fact]
    public async Task InvalidRequest_ReturnsValidationError()
    {
        var request = new CreateTag.Request(string.Empty, null, null);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("validation failure should be reported as error");

        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("errors should only contain one validation error");
        error.Type.Should().Be(ErrorType.Validation);
        error.Code.Should().Be("Name");
        error.Description.Should().Be("Required");
    }

    [Fact]
    public async Task DuplicateName_ReturnsConflictError()
    {
        await Application.AddAsync(new TagBuilder().WithName("super tag").Build());

        var request = new CreateTag.Request("SUPER TAG  ", null, null);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("conflict should be an error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.Conflict);
        error.Code.Should().Be("Name");
    }

    [Fact]
    public async Task CreatesTag()
    {
        var dt = new DateTimeOffset(2023, 12, 13, 13, 21, 15, TimeSpan.Zero);
        Application.SetDateTime(dt);
        var request = new CreateTag.Request("hey", "#ffffff", "pi pi-check");

        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        var tagDto = result.Value;
        tagDto.Id.Should().NotBeEmpty();
        tagDto.Name.Should().Be("hey");
        tagDto.Color.Should().Be("#ffffff");
        tagDto.Icon.Should().Be("pi pi-check");

        var expectedTag = new Tag
        {
            Id = tagDto.Id,
            Name = "hey",
            Color = "#ffffff",
            Icon = "pi pi-check",
            CreatedBy = TestConstants.DefaultUserId,
            CreatedOn = dt,
            LastModifiedBy = TestConstants.DefaultUserId,
            LastModifiedOn = dt,
        };
        var tag = await Application.FindAsync<Tag>(tagDto.Id);
        tag.Should().BeEquivalentTo(expectedTag);
    }

    [Fact]
    public async Task SanitisesValues()
    {
        var request = new CreateTag.Request("   UNtrimmed    ", "#ffFF09", "   pi pi-check    ");
        var result = await Application.SendAsync(request);

        var tagDto = result.Value;
        tagDto!.Name.Should().Be("UNtrimmed");
        tagDto.Color.Should().Be("#ffff09");
        tagDto.Icon.Should().Be("pi pi-check");

        var tag = await Application.FindAsync<Tag>(tagDto.Id);
        tag!.Name.Should().Be("UNtrimmed");
        tag.Color.Should().Be("#ffff09");
        tag.Icon.Should().Be("pi pi-check");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task PersistsNullInsteadOfWhitespace(string? value)
    {
        var request = new CreateTag.Request("some tag", value, value);
        var result = await Application.SendAsync(request);

        var tagDto = result.Value;
        tagDto!.Color.Should().BeNull();
        tagDto.Icon.Should().BeNull();

        var tag = await Application.FindAsync<Tag>(tagDto.Id);
        tag!.Color.Should().BeNull();
        tag.Icon.Should().BeNull();
    }

    [Fact]
    public async Task DuplicateNameOfOtherUser_CreatesTag()
    {
        await Application.AddAsync(new TagBuilder().WithName("super tag").Build());

        Application.SetUserId("other_user");
        var request = new CreateTag.Request("super tag", null, null);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();

        var tag = await Application.FindAsync<Tag>(result.Value.Id);
        tag!.Name.Should().Be("super tag");
    }
}