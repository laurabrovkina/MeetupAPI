using FastEndpoints;
using MeetupAPI.MinimalApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MinimalApi.Mappings;
using MinimalApi.Models;
using MinimalApi.Responses;
using MinimalApi.Services;

namespace MeetupAPI.MinimalApi.Endpoints;

[HttpPost("api/v2/account/register"), AllowAnonymous]
public class RegisterAccountEndpoint : Endpoint<RegisterUserDtoRequest, RegisterUserDtoResponse>
{
    private readonly IAccountService _accountService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public RegisterAccountEndpoint(IAccountService accountService,
    IPasswordHasher<User> passwordHasher)
    {
        _accountService = accountService;
        _passwordHasher = passwordHasher;
    }

    public override async Task HandleAsync(RegisterUserDtoRequest request, CancellationToken ct)
    {
        var user = request.ToUser();

        var passwordHash = _passwordHasher.HashPassword(user, request.Password);
        user.PasswordHash = passwordHash;

        await _accountService.CreateAsync(user);
        
        var userResponse = request.ToRegisterUserDtoResponse();
        await SendCreatedAtAsync<GetUserEndpoint>(
            new { Id = user.Id }, userResponse, generateAbsoluteUrl: true, cancellation: ct
        );
    }
}
