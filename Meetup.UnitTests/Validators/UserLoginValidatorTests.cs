using FluentValidation.TestHelper;
using MeetupAPI.Models;
using Validators;
using Xunit;

namespace Meetup.UnitTests.Validators;

public class UserLoginValidatorTests
{
    private readonly UserLoginValidator _validator = new();
    
    [Fact]
    public void Should_Not_Have_Validation_Error_For_Valid_UserLoginDto()
    {
        var sut = new UserLoginRequest
        {
            Email = "john.doe@example.com",
            Password = "012345",
            ConfirmPassword = "012345"
        };

        var result = _validator.TestValidate(sut);
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Throw_Error_When_Email_Is_Null_Or_Empty(string? invalidEmail)
    {
        // Arrange
        var sut = new UserLoginRequest
        {
            Email = invalidEmail,
            Password = "012345",
            ConfirmPassword = "012345"
        };

        // Act
        var result = _validator.TestValidate(sut);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
    [Theory]
    [InlineData("0")]
    [InlineData("12")]
    [InlineData("abc")]
    [InlineData("1000")]
    [InlineData("1000t")]
    public void Should_Throw_Error_When_Password_Is_Shorted_Then_Required(string password)
    {
        var sut = new UserLoginRequest
        {
            Email = "john.doe@example.com",
            Password = password,
            ConfirmPassword = password
        };

        var result = _validator.TestValidate(sut);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
    
    [Theory]
    [InlineData("abcdef", "abcdef1")]
    [InlineData("123456", "123567")]
    public void Should_Throw_Error_When_Password_And_ConfirmPassword_Do_Not_Match(string password, string confirmPassword)
    {
        var sut = new UserLoginRequest
        {
            Email = "john.doe@example.com",
            Password = password,
            ConfirmPassword = confirmPassword
        };

        var result = _validator.TestValidate(sut);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}