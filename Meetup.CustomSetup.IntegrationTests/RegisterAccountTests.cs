using FluentAssertions;
using Meetup.CustomSetup.IntegrationTests.FakeAccount;
using MeetupAPI.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Meetup.CustomSetup.IntegrationTests;

public class RegisterAccountTests : TestBase
{
    [Fact]
    public async Task Create_ReturnsCreated_WhenRequestIsCorrect()
    {
        // Arrange
        const string url = "/api/account/register";
        var testingServiceScope = new TestingServiceScope();
        var fakeAccount = new FakeAccountForRegisterUserDto().Generate();

        // Act
        await testingServiceScope.PostAsync<RegisterUserDto>(fakeAccount, url);

        var accountCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.Users
            .FirstOrDefaultAsync());

        // Assert
        accountCreated.Email.Should().Be(fakeAccount.Email);
        accountCreated.RoleId.Should().Be(fakeAccount.RoleId);
        accountCreated.Nationality.Should().Be(fakeAccount.Nationality);
        accountCreated.DateOfBirth.Should().Be(fakeAccount.DateOfBirth);
    }
}
