using Chores.Application.IntegrationTests.Common;
using Chores.Application.IntegrationTests.Common.Builders.Tags;
using Chores.Application.Modules.Chores.Iterations;
using Chores.Domain.Models.Chores;
using ErrorOr;

namespace Chores.Application.IntegrationTests.Modules.Chores.Iterations;

[Collection(nameof(ApplicationFixture))]
public sealed class DeleteChoreIterationTests : ApplicationTestBase
{
    public DeleteChoreIterationTests(ApplicationFixture application) : base(application)
    {
    }

    [Fact]
    public async Task InvalidChoreId_ReturnsNotFound()
    {
        var request = new DeleteChoreIteration.Request(new Guid("64D88E6F-EE51-40F2-993E-09FC31B1ECA1"), Guid.Empty);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("non-existing chore ID should result in error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("ChoreId");
    }

    [Fact]
    public async Task InvalidIterationId_ReturnsNotFound()
    {
        var choreId = new Guid("3FC0060C-D0D5-487C-A0E8-F34CF9B04302");
        await Application.AddAsync(new ChoreBuilder().WithId(choreId).Build());

        var request = new DeleteChoreIteration.Request(choreId, new Guid("76B5EE72-0902-45D5-A75F-B63C45FC9EEB"));
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("non-existing iteration ID should result in error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("IterationId");
    }

    [Fact]
    public async Task InaccessibleChoreId_ReturnsNotFound()
    {
        var choreId = new Guid("3FC0060C-D0D5-487C-A0E8-F34CF9B04302");
        await Application.AddAsync(new ChoreBuilder().WithId(choreId).Build());
        Application.SetUserId("other_user");

        var request = new DeleteChoreIteration.Request(choreId, Guid.Empty);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("inaccessible chore ID should result in error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("ChoreId");
    }

    [Fact]
    public async Task InaccessibleIterationId_ReturnsNotFound()
    {
        var choreId = new Guid("AA34B4DE-8E45-4D2A-9D84-35C25A3F9464");
        var iterationId = new Guid("8DFCDDB0-CBDF-45BF-A914-447D17EFFF3D");
        await Application.AddAsync(
            new ChoreBuilder()
                .WithId(choreId)
                .WithIterations([new ChoreIterationBuilder().WithId(iterationId)])
                .Build()
        );
        Application.SetUserId("other_user");

        var request = new DeleteChoreIteration.Request(choreId, iterationId);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("inaccessible iteration ID should result in error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("ChoreId");
    }

    [Fact]
    public async Task IterationIdOfOtherChore_ReturnsNotFound()
    {
        var chore1Id = new Guid("AA34B4DE-8E45-4D2A-9D84-35C25A3F9464");
        var iterationId = new Guid("8DFCDDB0-CBDF-45BF-A914-447D17EFFF3D");
        await Application.AddAsync(
            new ChoreBuilder()
                .WithId(chore1Id)
                .WithIterations([new ChoreIterationBuilder().WithId(iterationId)])
                .Build()
        );
        var chore2Id = new Guid("70292690-4826-4298-A31A-19CD107A7F5C");
        await Application.AddAsync(new ChoreBuilder().WithId(chore2Id).Build());

        var request = new DeleteChoreIteration.Request(chore2Id, iterationId);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeTrue("inaccessible iteration ID should result in error");
        var error = result.ErrorsOrEmptyList.SingleOrDefault();
        error.Should().NotBeNull("should contain exactly one error");
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be("IterationId");
    }

    [Fact]
    public async Task DeletesIteration()
    {
        var choreId = new Guid("AA34B4DE-8E45-4D2A-9D84-35C25A3F9464");
        var iterationId = new Guid("8DFCDDB0-CBDF-45BF-A914-447D17EFFF3D");
        await Application.AddAsync(
            new ChoreBuilder()
                .WithId(choreId)
                .WithIterations([new ChoreIterationBuilder().WithId(iterationId)])
                .Build()
        );

        var request = new DeleteChoreIteration.Request(choreId, iterationId);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Deleted);

        var chore = await Application.FindAsync<Chore>(c => c.Id == choreId, c => c.Iterations);
        chore!.Iterations.Should().BeEmpty();
    }

    [Fact]
    public async Task DeletesOnlyRequestedIteration()
    {
        var choreId = new Guid("AA34B4DE-8E45-4D2A-9D84-35C25A3F9464");
        var iteration1Id = new Guid("8DFCDDB0-CBDF-45BF-A914-447D17EFFF3D");
        var iteration2Id = new Guid("0716AABD-37C9-481A-AD33-6E38B66A7BCE");
        await Application.AddAsync(
            new ChoreBuilder()
                .WithId(choreId)
                .WithIterations([
                    new ChoreIterationBuilder().WithId(iteration1Id),
                    new ChoreIterationBuilder().WithId(iteration2Id),
                ])
                .Build()
        );

        var request = new DeleteChoreIteration.Request(choreId, iteration1Id);
        var result = await Application.SendAsync(request);

        result.IsError.Should().BeFalse();
        result.Value.Should().Be(Result.Deleted);

        var chore = await Application.FindAsync<Chore>(c => c.Id == choreId, c => c.Iterations);
        chore!.Iterations
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .BeEquivalentTo(new { Id = iteration2Id });
    }
}