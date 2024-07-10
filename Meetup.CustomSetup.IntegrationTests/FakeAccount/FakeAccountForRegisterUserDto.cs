using MeetupAPI.Models;
using Bogus;

namespace Meetup.CustomSetup.IntegrationTests.FakeAccount;

public class FakeAccountForRegisterUserDto : Faker<RegisterUserDto>
{
    // if you want default values on any of your properties (e.g. an int between a certain range or a date always in the past), you can add `RuleFor` lines describing those defaults
    //RuleFor(r => r.ExampleIntProperty, r => r.Random.Number(50, 100000));
    //RuleFor(r => r.ExampleDateProperty, r => r.Date.Past());
    public FakeAccountForRegisterUserDto()
    {
        new Faker<RegisterUserDto>()
            .RuleFor(x => x.Email, faker => faker.Person.Email)
            .RuleFor(x => x.DateOfBirth, f => f.Date.Past())
            .RuleFor(x => x.RoleId, f => f.Random.Number(1, 3));
    }

}
