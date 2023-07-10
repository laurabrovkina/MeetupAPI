using MeetupAPI.MinimalApi;
using MinimalApi.Models;

namespace MinimalApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {

        private readonly MeetupContext _meetupContext;

        public AccountRepository(MeetupContext meetupContext)
        {
            _meetupContext = meetupContext;
        }

        public async Task<User> CreateAsync(User User)
        {
            await _meetupContext.Users.AddAsync(User);
            await _meetupContext.SaveChangesAsync();

            return User;
        }
    }
}