using MeetupAPI.MinimalApi;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Models;

namespace MinimalApi.Repositories;

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

    public async Task<User> GetAsync(int id) => await _meetupContext.Users.FirstOrDefaultAsync(x => x.Id == id);
}