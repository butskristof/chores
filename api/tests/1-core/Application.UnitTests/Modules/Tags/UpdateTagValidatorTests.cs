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

        result.ShouldHaveValidationErrorFor(r => r.Name);
    }
}