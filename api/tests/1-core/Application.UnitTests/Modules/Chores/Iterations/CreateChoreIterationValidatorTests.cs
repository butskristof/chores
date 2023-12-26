using Chores.Application.Modules.Chores.Iterations;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Time.Testing;

namespace Chores.Application.UnitTests.Modules.Chores.Iterations;

public sealed class CreateChoreIterationValidatorTests
{
    #region construction

    private readonly FakeTimeProvider _timeProvider = new();
    private readonly CreateChoreIteration.Validator _sut;

    public CreateChoreIterationValidatorTests()
    {
        _sut = new CreateChoreIteration.Validator(_timeProvider);
    }

    #endregion

    [Fact]
    public void Date_InTheFuture_Fails()
    {
        _timeProvider.SetUtcNow(new DateTimeOffset(2023, 12, 23, 17, 35, 23, TimeSpan.Zero));
        var request = new CreateChoreIteration.Request(Guid.Empty,
            new DateTimeOffset(2023, 12, 24, 0, 0, 0, TimeSpan.Zero), null);
        var result = _sut.TestValidate(request);
        result.ShouldHaveValidationErrorFor(r => r.Date);
    }

    [Theory]
    [InlineData(23)]
    [InlineData(22)]
    public void Date_TodayOrInThePast_Passes(int day)
    {
        _timeProvider.SetUtcNow(new DateTimeOffset(2023, 12, 23, 17, 35, 23, TimeSpan.Zero));
        var date = new DateTimeOffset(2023, 12, day, 0, 0, 0, TimeSpan.Zero);
        var request = new CreateChoreIteration.Request(Guid.Empty, date, null);
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(r => r.Date);
    }
}