using Chores.Application.IntegrationTests.Common;
using Chores.Application.Modules.Tags;
using Chores.Domain.Models;
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
        var request = new CreateTag.Request(string.Empty);

        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("validation failure should be reported as error");

        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("errors should only contain one validation error");
        error.Type.Should().Be(ErrorType.Validation);
        error.Code.Should().Be(nameof(CreateTag.Request.Name));
    }

    [Fact]
    public async Task DuplicateName_ReturnsConflictError()
    {
        {
            var tag = new Tag { Name = "super tag" };
            await Application.AddAsync(tag);
        }
        
        var request = new CreateTag.Request("SUPER TAG  ");
        var result = await Application.SendAsync(request);
        
        result.IsError.Should().BeTrue();
        result.ErrorsOrEmptyList.Single().Type.Should().Be(ErrorType.Conflict);
    }

    [Theory]
    [InlineData("  untrimmed     ", "untrimmed")]
    [InlineData("hey", "hey")]
    public async Task CreatesTag(string input, string expected)
    {
        var request = new CreateTag.Request(input);

        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        var tagDto = result.Value;
        tagDto.Id.Should().NotBeEmpty();
        tagDto.Name.Should().Be(expected);

        var tag = await Application.FindAsync<Tag>(tagDto.Id);
        tag.Should().NotBeNull();
        tag!.Id.Should().Be(tagDto.Id);
        tag.Name.Should().Be(expected);
    }
}