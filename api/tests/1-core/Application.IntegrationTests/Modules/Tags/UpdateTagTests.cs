using Chores.Application.IntegrationTests.Common;
using Chores.Application.Modules.Tags;
using Chores.Domain.Models;
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
        error.Code.Should().Be(nameof(UpdateTag.Request.Name));
    }

    [Fact]
    public async Task InvalidId_ReturnsNotFoundError()
    {
        var request = new UpdateTag.Request(new Guid("F142362E-E424-469D-B495-3FD360A88CE5"), "valid name");
        var result = await Application.SendAsync(request);
        result.IsError.Should().BeTrue();
        result.ErrorsOrEmptyList.Single().Type.Should().Be(ErrorType.NotFound);
    }

    [Fact]
    public async Task DuplicateName_ReturnsConflictError()
    {
        var id = new Guid("F9123C25-0597-481D-BA97-AA1B9BC884A4");
        {
            await Application.AddAsync(new Tag { Name = "some name" });
            await Application.AddAsync(new Tag { Id = id, Name = "other name" });
        }

        var request = new UpdateTag.Request(id, "some name");
        var result = await Application.SendAsync(request);
        result.IsError.Should().BeTrue();
        result.ErrorsOrEmptyList.Single().Type.Should().Be(ErrorType.Conflict);
    }

    [Theory]
    [InlineData("some name", "some name")]
    [InlineData("     some name", "some name")]
    public async Task UpdatesTag(string input, string expected)
    {
        var id = new Guid("A6411F21-A841-450D-BD52-D41A2F36D8B8");
        {
            await Application.AddAsync(new Tag { Id = id, Name = "to update" });
        }
        var request = new UpdateTag.Request(id, input);
        var result = await Application.SendAsync(request);
        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Updated);
        var tag = await Application.FindAsync<Tag>(id);
        tag!.Name.Should().Be(expected);
    }
}