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
        var request = new CreateChoreIteration.Request(Guid.Empty, new DateOnly(2023, 12, 24), null);
        var result = _sut.TestValidate(request);
        result.ShouldHaveValidationErrorFor(r => r.Date);
    }

    [Theory]
    [InlineData("2023-12-23")]
    [InlineData("2023-12-22")]
    public void Date_TodayOrInThePast_Passes(string dateString)
    {
        _timeProvider.SetUtcNow(new DateTimeOffset(2023, 12, 23, 17, 35, 23, TimeSpan.Zero));
        var date = DateOnly.Parse(dateString);
        var request = new CreateChoreIteration.Request(Guid.Empty, date, null);
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(r => r.Date);
    }
}