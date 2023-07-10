using MinimalApi.Models;

namespace MinimalApi.Repositories;

public interface IAccountRepository
{
    Task<User> CreateAsync(User registerUserDto);
    Task<User> GetAsync(int id);
}
