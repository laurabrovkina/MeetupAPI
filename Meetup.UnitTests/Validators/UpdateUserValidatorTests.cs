using FluentValidation.TestHelper;
using MeetupAPI.Validators;
using MeetupAPI.Models;
using MeetupAPI.Entities;
using Xunit;

namespace MeetupAPI.Facts.Validators;

[Collection("Database collection")]
public class UpdateUserValidatorTests
{
    DatabaseFixture _fixture;
    private readonly MeetupContext meetupContext;
    public UpdateUserValidatorTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        meetupContext = _fixture._meetupContext;
    }

    [Fact]
    public void ShouldHaveValidationError_WhenRoleIdDoesNotExist()
    {
        // Arrange           
        var validator = new UpdateUserValidator(meetupContext);
        var updateUserDto = new UpdateUserDto { RoleId = 999 }; // Assuming 999 is an invalid RoleId

        // Act
        var result = validator.TestValidate(updateUserDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RoleId)
            .WithErrorMessage("Role doesn't exist");
    }

    [Fact]
    public void ShouldHaveValidationError_WhenEmailDoesNotExist()
    {
        // Arrange
        var validator = new UpdateUserValidator(meetupContext);
        var updateUserDto = new UpdateUserDto { Email = "nonexistent@example.com" }; // Assuming the email doesn't exist in the database

        // Act
        var result = validator.TestValidate(updateUserDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("This email address doesn't exist");
    }

    [Fact]
    public void ShouldNotHaveValidationError_WhenRoleIdAndEmailExist()
    {
        // Arrange
        var validator = new UpdateUserValidator(meetupContext);
        var updateUserDto = new UpdateUserDto { RoleId = 1, Email = "existing@example.com" }; // Assuming RoleId and email exist in the database

        // Act
        var result = validator.TestValidate(updateUserDto);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.RoleId);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }
}