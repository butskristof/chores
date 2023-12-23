using Chores.Application.IntegrationTests.Common;
using Chores.Application.IntegrationTests.Common.Builders.Tags;
using Chores.Application.Modules.Chores.Iterations;
using Chores.Domain.Models.Chores;
using ErrorOr;

namespace Chores.Application.IntegrationTests.Modules.Chores.Iterations;

[Collection(nameof(ApplicationFixture))]
public sealed class CreateChoreIterationTests : ApplicationTestBase
{
    public CreateChoreIterationTests(ApplicationFixture application) : base(application)
    {
    }

    [Fact]
    public async Task InvalidRequest_ReturnsValidationError()
    {
        Application.SetDateTime(new DateTimeOffset(2023, 12, 23, 17, 11, 10, TimeSpan.Zero));
        var request = new CreateChoreIteration.Request(Guid.Empty, new DateOnly(2023, 12, 31), null);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("date in the future should not be allowed");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.Validation);
        error.Code.Should().Be("Date");
        error.Description.Should().Be("Invalid");
    }

    [Fact]
    public async Task InvalidChoreId_ReturnsNotFound()
    {
        Application.SetDateTime(new DateTimeOffset(2023, 12, 23, 17, 14, 32, TimeSpan.Zero));
        var request = new CreateChoreIteration.Request(new Guid("96E2F704-B5CA-4323-954E-31BB4B0A562A"),
            new DateOnly(2023, 12, 17), null);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("non-existing ID should result in error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("ChoreId");
    }

    [Fact]
    public async Task InaccessibleChoreId_ReturnsNotFound()
    {
        var id = new Guid("58B4C308-FB42-44A3-95DE-15FC1CCFABDD");
        await Application.AddAsync(new ChoreBuilder().WithId(id).Build());
        Application.SetUserId("other_user");
        Application.SetDateTime(new DateTimeOffset(2023, 12, 23, 17, 14, 32, TimeSpan.Zero));
        var request = new CreateChoreIteration.Request(id, new DateOnly(2023, 12, 17), null);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("non-existing ID should result in error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("ChoreId");
    }

    [Fact]
    public async Task CreatesIteration()
    {
        var id = new Guid("0B60F96F-14FF-4128-B717-ADFDD9E92445");
        await Application.AddAsync(new ChoreBuilder().WithId(id).Build());
        Application.SetDateTime(new DateTimeOffset(2023, 12, 23, 17, 17, 17, TimeSpan.Zero));
        var request = new CreateChoreIteration.Request(id, new DateOnly(2023, 12, 17), "some notes");
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Created);

        var chore = await Application.FindAsync<Chore>(c => c.Id == id, c => c.Iterations);
        var iteration = chore!.Iterations.SingleOrDefault();
        iteration.Should().NotBeNull("Iteration should be added to Chore");
        iteration!.Date.Should().Be(new DateOnly(2023, 12, 17));
        iteration.Notes.Should().Be("some notes");
    }

    [Fact]
    public async Task RetainsExistingIterations()
    {
        await Task.Delay(1);
        true.Should().BeFalse();
    }
}