using Chores.Application.IntegrationTests.Common;
using Chores.Application.Modules.Tags;

namespace Chores.Application.IntegrationTests.Modules.Tags;

[Collection(nameof(TestFixture))]
public sealed class CreateTagTests : TestBase
{
    public CreateTagTests(TestFixture fixture) : base(fixture)
    {
    }
    
    [Fact]
    public async Task Test()
    {
        var request = new CreateTag.Request("hey");
        var response = await Fixture.SendAsync(request);
        Assert.False(response.IsError);
        Assert.Equal("hey", response.Value.Name);
    }
    
    [Fact]
    public async Task Test2()
    {
        var request = new CreateTag.Request("hey");
        var response = await Fixture.SendAsync(request);
        Assert.False(response.IsError);
        Assert.Equal("hey", response.Value.Name);
    }
}