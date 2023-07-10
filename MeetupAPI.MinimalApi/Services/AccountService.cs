using MinimalApi.Models;
using MinimalApi.Repositories;

namespace MinimalApi.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<User> CreateAsync(User user)
    {
        return await _accountRepository.CreateAsync(user);
    }

    public async Task<User?> GetAsync(int id)
    {
        var user = await _accountRepository.GetAsync(id);
        return user;
    }
}
