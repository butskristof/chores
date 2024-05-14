using Chores.Application.Modules.Tags;
using Chores.Application.UnitTests.Common;
using FluentValidation.TestHelper;

namespace Chores.Application.UnitTests.Modules.Tags;

public sealed class UpdateTagValidatorTests
{
    private readonly UpdateTag.Validator _sut = new();

    private sealed class UpdateTagBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _name = "some tag";
        private string? _color = null;
        private string? _icon = null;

        internal UpdateTagBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }
    
        internal UpdateTagBuilder WithName(string name)
        {
            _name = name;
            return this;
        }
    
        internal UpdateTagBuilder WithColor(string? color)
        {
            _color = color;
            return this;
        }
    
        internal UpdateTagBuilder WithIcon(string? icon)
        {
            _icon = icon;
            return this;
        }
    
        internal UpdateTag.Request Build() => new(_id, _name, _color, _icon);
    
        public static implicit operator UpdateTag.Request(UpdateTagBuilder builder) => builder.Build();
    }
    
    #region Name

    [Theory]
    [ClassData(typeof(TestData.EmptyStrings))]
    public void Name_NullOrWhitespace_Fails(string value)
    {
        var request = new UpdateTagBuilder().WithName(value);
        var result = _sut.TestValidate(request);
        result
            .ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage("Required");
    }

    [Fact]
    public void Name_TooLong_Fails()
    {
        var request = new UpdateTagBuilder().WithName(new string('*', 513));
        var result = _sut.TestValidate(request);
        result
            .ShouldHaveValidationErrorFor(r => r.Name)
            .WithErrorMessage("Invalid");
    }

    [Fact]
    public void Name_Valid_Passes()
    {
        var request = new UpdateTagBuilder().WithName("super tag");
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
        var request = new UpdateTagBuilder().WithColor(value);
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
        var request = new UpdateTagBuilder().WithColor(value);
        var result = _sut.TestValidate(request);
        result
            .ShouldNotHaveValidationErrorFor(r => r.Color);
    }

    [Theory]
    [ClassData(typeof(TestData.ValidColors))]
    public void Color_ValidHex_Passes(string value)
    {
        var request = new UpdateTagBuilder().WithColor(value);
        var result = _sut.TestValidate(request);
        result
            .ShouldNotHaveValidationErrorFor(r => r.Color);
    }

    #endregion

    #region Icon

    [Fact]
    public void Icon_TooLong_Fails()
    {
        var request = new UpdateTagBuilder().WithIcon(new string('*', 33));
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
        var request = new UpdateTagBuilder().WithIcon(value);
        var result = _sut.TestValidate(request);
        result
            .ShouldNotHaveValidationErrorFor(r => r.Icon);
    }

    [Fact]
    public void Icon_WithValue_Passes()
    {
        var request = new UpdateTagBuilder().WithIcon("pi pi-check");
        var result = _sut.TestValidate(request);
        result
            .ShouldNotHaveValidationErrorFor(r => r.Icon);

    }

    #endregion
}