using Chores.Application.Modules.Tags;
using Chores.Application.UnitTests.Common;
using FluentValidation.TestHelper;

namespace Chores.Application.UnitTests.Modules.Tags;

public sealed class UpdateTagValidatorTests
{
    private readonly UpdateTag.Validator _sut = new();

    [Theory]
    [ClassData(typeof(TestData.EmptyStrings))]
    public void Name_NullOrWhitespace_Fails(string value)
    {
        var request = new UpdateTag.Request(Guid.Empty, value);
        var result = _sut.TestValidate(request);
        result
            .ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage("Required");
    }

    [Fact]
    public void Name_TooLong_Fails()
    {
        var request = new UpdateTag.Request(Guid.Empty, new string('*', 513));
        var result = _sut.TestValidate(request);
        result
            .ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage("Invalid");
    }

    [Fact]
    public void Name_Valid_Passes()
    {
        var request = new UpdateTag.Request(Guid.Empty, "super tag");
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(r => r.Name);
    }
}