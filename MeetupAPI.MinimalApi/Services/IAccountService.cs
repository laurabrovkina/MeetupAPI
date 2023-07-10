using MinimalApi.Models;

namespace MinimalApi.Services;

public interface IAccountService
{
    Task<User> CreateAsync(User request);
}