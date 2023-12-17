using Chores.Application.IntegrationTests.Common;
using Chores.Application.Modules.Chores;
using Chores.Domain.Models.Chores;

namespace Chores.Application.IntegrationTests.Modules.Chores;

[Collection(nameof(ApplicationFixture))]
public sealed class CreateChoreTests : ApplicationTestBase
{
    public CreateChoreTests(ApplicationFixture application) : base(application)
    {
    }

    [Fact]
    public async Task CreatesChore()
    {
        var dt = new DateTimeOffset(2023, 12, 17, 17, 50, 24, TimeSpan.Zero);
        Application.SetDateTime(dt);
        
        var request = new CreateChore.Request("do something", 5);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        var choreDto = result.Value;
        choreDto.Id.Should().NotBeEmpty();
        choreDto.Name.Should().Be("do something");
        choreDto.Interval.Should().Be(5);

        var expectedChore = new Chore
        {
            Id = choreDto.Id,
            Name = "do something",
            Interval = 5,
            CreatedBy = TestConstants.DefaultUserId,
            CreatedOn = dt,
            LastModifiedBy = TestConstants.DefaultUserId,
            LastModifiedOn = dt,
        };
        var chore = Application.FindAsync<Chore>(choreDto.Id);
        chore.Should().BeEquivalentTo(expectedChore);
    }
}