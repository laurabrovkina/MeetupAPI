using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Entities;
using Microsoft.IdentityModel.Tokens;

namespace Identity;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(JwtOptions jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    public string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, user.Role.RoleName),
            new(ClaimTypes.Name, user.Email),
        };

        if (!string.IsNullOrEmpty(user.Nationality))
        {
            claims.Add(
                new Claim("Nationality", user.Nationality)
                );
        }
        
        if (user.DateOfBirth is not null)
        {
            claims.Add(
                new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("dd-MM-yyyy"))
            );
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.JwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtOptions.JwtIssuer,
            _jwtOptions.JwtIssuer,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.JwtExpireInMinutes),
            signingCredentials: creds
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}