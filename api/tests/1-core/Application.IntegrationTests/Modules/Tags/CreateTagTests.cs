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

        Assert.True(result.IsError);
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        Assert.Equal(ErrorType.Validation, error.Type);
    }

    [Fact]
    public async Task UntrimmedInput_SavesAsTrimmed()
    {
        var request = new CreateTag.Request("  untrimmed     ");

        var result = await Application.SendAsync(request);

        Assert.False(result.IsError);
        var tagDto = result.Value;
        Assert.Equal("untrimmed", tagDto.Name);

        var tag = await Application.FindAsync<Tag>(tagDto.Id);
        Assert.NotNull(tag);
        Assert.Equal(tagDto.Id, tag.Id);
        Assert.Equal("untrimmed", tag.Name);
    }

    [Fact]
    public async Task CreatesTag()
    {
        var request = new CreateTag.Request("hey");

        var result = await Application.SendAsync(request);

        Assert.False(result.IsError);
        var tagDto = result.Value;
        Assert.NotEqual(Guid.Empty, tagDto.Id);
        Assert.Equal("hey", tagDto.Name);
        
        var tag = await Application.FindAsync<Tag>(tagDto.Id);
        Assert.NotNull(tag);
        Assert.Equal(tagDto.Id, tag.Id);
        Assert.Equal("hey", tag.Name);
    }
}