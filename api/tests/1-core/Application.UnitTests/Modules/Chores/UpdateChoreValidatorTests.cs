using Chores.Application.Modules.Chores;
using Chores.Application.UnitTests.Common;
using FluentValidation.TestHelper;

namespace Chores.Application.UnitTests.Modules.Chores;

public sealed class UpdateChoreValidatorTests
{
    private readonly UpdateChore.Validator _sut = new();

    [Theory]
    [ClassData(typeof(TestData.EmptyStrings))]
    public void Name_NullOrWhitespace_Fails(string value)
    {
        var request = new UpdateChore.Request(Guid.Empty, value, 1);
        var result = _sut.TestValidate(request);
        result
            .ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage("Required");
    }

    [Fact]
    public void Name_TooLong_Fails()
    {
        var request = new UpdateChore.Request(Guid.Empty, new string('*', 513), 1);
        var result = _sut.TestValidate(request);
        result
            .ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage("Invalid");
    }

    [Fact]
    public void Name_Valid_Passes()
    {
        var request = new UpdateChore.Request(Guid.Empty, "super tag", 1);
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(r => r.Name);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Interval_NegativeOrZero_Fails(int input)
    {
        var request = new UpdateChore.Request(Guid.Empty, "valid name", input);
        var result = _sut.TestValidate(request);
        result.ShouldHaveValidationErrorFor(r => r.Interval);
    }

    [Fact]
    public void Interval_Valid_Passes()
    {
        var request = new UpdateChore.Request(Guid.Empty, "valid name", 1);
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(r => r.Interval);
    }
}