using Meetup.CustomSetup.IntegrationTests.FakeAccount;
using Xunit;

namespace Meetup.CustomSetup.IntegrationTests;

public class RegisterAccountTests
{
    [Fact]
    public async Task can_add_new_recipe_to_db()
    {
        // Arrange
        var testingServiceScope = new TestingServiceScope();
        var fakeAccount = new FakeAccountForRegisterUserDto().Generate();

        // Act
        var command = new AddRecipe.Command(fakeAccount);
        var recipeReturned = await testingServiceScope.SendAsync(command);
        var recipeCreated = await testingServiceScope.ExecuteDbContextAsync(db => db.Recipes
            .FirstOrDefaultAsync(r => r.Id == recipeReturned.Id));

        // Assert
        recipeReturned.Email.Should().Be(fakeAccount.Email);
    }
}
