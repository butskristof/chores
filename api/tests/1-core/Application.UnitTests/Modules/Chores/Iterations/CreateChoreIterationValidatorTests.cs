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
        _timeProvider.SetUtcNow(new DateTimeOffset(2023, 12, 23, 7, 35, 23, TimeSpan.Zero));
        var request = new CreateChoreIteration.Request(Guid.Empty, new DateOnly(2023, 12, 24), null);
        var result = _sut.TestValidate(request);
        result.ShouldHaveValidationErrorFor(r => r.Date);
    }

    [Theory]
    [InlineData(23)]
    [InlineData(22)]
    public void Date_TodayOrInThePast_Passes(int day)
    {
        _timeProvider.SetUtcNow(new DateTimeOffset(2023, 12, 23, 17, 35, 23, TimeSpan.Zero));
        var date = new DateOnly(2023, 12, day);
        var request = new CreateChoreIteration.Request(Guid.Empty, date, null);
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(r => r.Date);
    }

    [Fact]
    public void Date_Today_InEarlierTimeZone_Passes()
    {
        // 25/12/2023 10:00:00 UTC
        _timeProvider.SetUtcNow(new DateTimeOffset(2023, 12, 25, 10, 0, 0, TimeSpan.Zero));
        // 26/12/2023 00:00:00 +14
        // if we only regard the date, this is the same day since the validator should covert the +13 timestamp to UTC
        // and compare the dates 
        var date = new DateOnly(2023, 12, 26);
        var request = new CreateChoreIteration.Request(Guid.Empty, date, null);
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(r => r.Date);
    }
    
    [Fact]
    public void Date_Future_InEarlierTimeZone_Fails()
    {
        // 25/12/2023 10:59:59 UTC
        _timeProvider.SetUtcNow(new DateTimeOffset(2023, 12, 25, 9, 59, 59, TimeSpan.Zero));
        // 26/12/2023 00:00:00 +14
        // this should NOT be possible: the +13 timestamp is converted to UTC, which still results in 26/12, which in 
        // turn does NOT match the current time of the system
        var date = new DateOnly(2023, 12, 26);
        var request = new CreateChoreIteration.Request(Guid.Empty, date, null);
        var result = _sut.TestValidate(request);
        result.ShouldHaveValidationErrorFor(r => r.Date);
    }
}