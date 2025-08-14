using System;
using System.Threading.Tasks;
using Entities;
using FluentValidation.TestHelper;
using MeetupAPI.Models;
using Microsoft.EntityFrameworkCore;
using Validators;
using Xunit;

namespace Meetup.UnitTests.Validators;

public class RegisterUserValidatorTests
{
    private readonly MeetupContextFactory _meetupContextFactory;
    private readonly RegisterUserValidator _validator;

    public RegisterUserValidatorTests()
    {
        var options = new DbContextOptionsBuilder<MeetupContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _meetupContextFactory = new MeetupContextFactory(options);
        var context = _meetupContextFactory.CreateDbContext();
        _validator = new RegisterUserValidator(context);
    }
    
    [Fact]
    public void Should_Not_Have_Validation_Error_For_Valid_RegisterUserRequest()
    {
        // Arrange
        var sut = new RegisterUserRequest
        {
            Email = "john.doe@example.com"
        };

        // Act
        var result = _validator.TestValidate(sut);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public async Task Should_Throw_Error_When_Email_Already_Exists_In_Db()
    {
        // Arrange
        const string existingEmail = "mary.sue@example.com";

        await using (var context = _meetupContextFactory.CreateDbContext())
        {
            var user = new User
            {
                Id = 1,
                Email = existingEmail,
                FirstName = "Mary",
                LastName = "Sue",
                Nationality = "English",
                RoleId = 3
            };
            var user1 = new User
            {
                Id = 2,
                Email = "someone@example.com",
                FirstName = "Sam",
                LastName = "Sue",
                Nationality = "English",
                RoleId = 3
            };
            context.Users.AddRange(user, user1);
            await context.SaveChangesAsync();
        }
        
        var sut = new RegisterUserRequest
        {
            Email = existingEmail
        };

        // Act
        var result = _validator.TestValidate(sut);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
}

public class MeetupContextFactory : IDbContextFactory<MeetupContext>
{
    private readonly DbContextOptions<MeetupContext> _options;

    public MeetupContextFactory(DbContextOptions<MeetupContext> options)
    {
        _options = options;
    }

    public MeetupContext CreateDbContext()
    {
        return new MeetupContext(_options);
    }
}