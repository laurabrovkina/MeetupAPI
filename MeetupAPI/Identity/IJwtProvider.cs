using Entities;

namespace Identity;

public interface IJwtProvider
{
    string GenerateJwtToken(User user);
}