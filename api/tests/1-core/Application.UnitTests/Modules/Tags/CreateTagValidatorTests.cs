using Chores.Application.Modules.Tags;
using Chores.Application.UnitTests.Common;
using FluentValidation.TestHelper;

namespace Chores.Application.UnitTests.Modules.Tags;

public sealed class CreateTagValidatorTests
{
    private readonly CreateTag.Validator _sut = new();

    private sealed class CreateTagBuilder
    {
        private string _name = "some tag";
        private string? _color = null;
        private string? _icon = null;

        internal CreateTagBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        internal CreateTagBuilder WithColor(string? color)
        {
            _color = color;
            return this;
        }

        internal CreateTagBuilder WithIcon(string? icon)
        {
            _icon = icon;
            return this;
        }

        internal CreateTag.Request Build() => new(_name, _color, _icon);

        public static implicit operator CreateTag.Request(CreateTagBuilder builder) => builder.Build();
    }

    #region Name

    [Theory]
    [ClassData(typeof(TestData.EmptyStrings))]
    public void Name_Whitespace_Fails(string value)
    {
        var request = new CreateTagBuilder().WithName(value);
        var result = _sut.TestValidate(request);
        result
            .ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage("Required");
    }

    [Fact]
    public void Name_TooLong_Fails()
    {
        var request = new CreateTagBuilder().WithName(new string('*', 513));
        var result = _sut.TestValidate(request);
        result
            .ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage("Invalid");
    }

    [Fact]
    public void Name_Valid_Passes()
    {
        var request = new CreateTagBuilder().WithName("super tag");
        var result = _sut.TestValidate(request);
        result.ShouldNotHaveValidationErrorFor(r => r.Name);
    }

    #endregion

    #region Color
    
    // for now, only 7-char regex strings are allowed
    // TODO update to support all css color possibilities (names, hsl, rgba, ...)

    [Theory]
    [ClassData(typeof(TestData.InvalidColors))]
    public void Color_InvalidHex_Fails(string value)
    {
        var request = new CreateTagBuilder().WithColor(value);
        var result = _sut.TestValidate(request);
        result
            .ShouldHaveValidationErrorFor(r => r.Color)
            .WithErrorMessage("Invalid");
    }

    [Theory]
    [ClassData(typeof(TestData.EmptyStrings))]
    [InlineData(null)]
    public void Color_NullOrWhitespace_Passes(string? value)
    {
        var request = new CreateTagBuilder().WithColor(value);
        var result = _sut.TestValidate(request);
        result
            .ShouldNotHaveValidationErrorFor(r => r.Color);
    }

    [Theory]
    [ClassData(typeof(TestData.ValidColors))]
    public void Color_ValidHex_Passes(string value)
    {
        var request = new CreateTagBuilder().WithColor(value);
        var result = _sut.TestValidate(request);
        result
            .ShouldNotHaveValidationErrorFor(r => r.Color);
    }

    #endregion

    #region Icon

    [Fact]
    public void Icon_TooLong_Fails()
    {
        var request = new CreateTagBuilder().WithIcon(new string('*', 33));
        var result = _sut.TestValidate(request);
        result
            .ShouldHaveValidationErrorFor(r => r.Icon)
            .WithErrorMessage("Invalid");
    }

    [Theory]
    [ClassData(typeof(TestData.EmptyStrings))]
    [InlineData(null)]
    public void Icon_NullOrWhitespace_Passes(string? value)
    {
        var request = new CreateTagBuilder().WithIcon(value);
        var result = _sut.TestValidate(request);
        result
            .ShouldNotHaveValidationErrorFor(r => r.Icon);
    }

    [Fact]
    public void Icon_WithValue_Passes()
    {
        var request = new CreateTagBuilder().WithIcon("pi pi-check");
        var result = _sut.TestValidate(request);
        result
            .ShouldNotHaveValidationErrorFor(r => r.Icon);

    }

    #endregion
}