using Chores.Application.Common.Configuration;
using FluentValidation.TestHelper;

namespace Chores.Application.UnitTests.Common.Configuration;

public sealed class AuthenticationSettingsValidatorTests
{
    private readonly AuthenticationSettingsValidator _sut = new();

    [Theory]
    [ClassData(typeof(TestData.EmptyStrings))]
    public void Issuer_NullOrWhitespace_Fails(string value)
    {
        var settings = new AuthenticationSettings { Issuer = value, Audiences = [] };
        var result = _sut.TestValidate(settings);
        result
            .ShouldHaveValidationErrorFor(r => r.Issuer)
            .WithErrorMessage("Required");
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("/relative")]
    [InlineData("invalid.com")]
    [InlineData("ftp://invalid.com")]
    public void Issuer_InvalidUri_Fails(string value)
    {
        var settings = new AuthenticationSettings { Issuer = value, Audiences = null! };
        var result = _sut.TestValidate(settings);
        result
            .ShouldHaveValidationErrorFor(r => r.Issuer)
            .WithErrorMessage("Invalid");
    }

    [Fact]
    public void Issuer_Valid_Passes()
    {
        var settings = new AuthenticationSettings { Issuer = "https://issuer.com", Audiences = null! };
        var result = _sut.TestValidate(settings);
        result.ShouldNotHaveValidationErrorFor(r => r.Issuer);
    }

    [Fact]
    public void Audiences_Null_Fails()
    {
        var settings = new AuthenticationSettings { Audiences = null!, Issuer = string.Empty };
        var result = _sut.TestValidate(settings);
        result
            .ShouldHaveValidationErrorFor(r => r.Audiences)
            .WithErrorMessage("Required");
    }

    [Fact]
    public void Audiences_Empty_Fails()
    {
        var settings = new AuthenticationSettings { Audiences = [], Issuer = string.Empty };
        var result = _sut.TestValidate(settings);
        result
            .ShouldHaveValidationErrorFor(r => r.Audiences)
            .WithErrorMessage("Required");
    }

    [Fact]
    public void Audiences_WithEmptyValue_Fails()
    {
        var settings = new AuthenticationSettings { Audiences = [string.Empty], Issuer = string.Empty };
        var result = _sut.TestValidate(settings);
        result
            // .ShouldHaveValidationErrorFor(r => r.Audiences)
            .ShouldHaveValidationErrorFor("Audiences[0]")
            .WithErrorMessage("Required");
    }

    [Theory]
    [InlineData("aud1")]
    [InlineData("aud1", "aud2")]
    public void Audiences_WithValues_Passes(params string[] values)
    {
        var settings = new AuthenticationSettings { Audiences = values, Issuer = string.Empty };
        var result = _sut.TestValidate(settings);
        result.ShouldNotHaveValidationErrorFor(r => r.Audiences);
    }
}
