using MeetupAPI.Entities;
using MeetupAPI.Extensions;
using MeetupAPI.Models;

namespace Meetup.CustomSetup.IntegrationTests.FakeAccount;

public class FakeUser
{
    public static User Generate(RegisterUserDto recipeForCreationDto)
    {
        return recipeForCreationDto.ToDomain();
    }

    public static User Generate()
    {
        return Generate(new FakeAccountForRegisterUserDto().Generate());
    }
}
