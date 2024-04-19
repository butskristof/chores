using Chores.Application.Common.Configuration;
using FluentValidation.TestHelper;

namespace Chores.Application.UnitTests.Common.Configuration;

public sealed class ClientSettingsValidatorTests
{
    private readonly ClientSettingsValidator _sut = new();

    [Fact]
    public void ClientUrls_Null_Fails()
    {
        var settings = new ClientSettings { ClientUrls = null! };
        var result = _sut.TestValidate(settings);
        result
            .ShouldHaveValidationErrorFor(r => r.ClientUrls)
            .WithErrorMessage("Required");
    }

    [Fact]
    public void ClientUrls_Empty_Passes()
    {
        var settings = new ClientSettings { ClientUrls = [] };
        var result = _sut.TestValidate(settings);
        result
            .ShouldNotHaveValidationErrorFor(r => r.ClientUrls);
    }
    
    [Theory]
    [ClassData(typeof(TestData.EmptyStrings))]
    public void ClientUrls_ValueEmpty_Fails(string value)
    {
        var settings = new ClientSettings { ClientUrls = [value] };
        var result = _sut.TestValidate(settings);
        result
            .ShouldHaveValidationErrorFor("ClientUrls[0]")
            .WithErrorMessage("Invalid");
    }
    
    [Theory]
    [InlineData("invalid")]
    [InlineData("/relative")]
    [InlineData("invalid.com")]
    [InlineData("ftp://invalid.com")]
    public void ClientUrls_InvalidUri_Fails(string value)
    {
        var settings = new ClientSettings { ClientUrls = [value] };
        var result = _sut.TestValidate(settings);
        result
            .ShouldHaveValidationErrorFor("ClientUrls[0]")
            .WithErrorMessage("Invalid");
    }
    
    [Fact]
    public void ClientUrls_ValidUri_Passes()
    {
        var settings = new ClientSettings { ClientUrls = ["http://valid.com"] };
        var result = _sut.TestValidate(settings);
        result.ShouldNotHaveValidationErrorFor(r => r.ClientUrls);
    }
}