using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using MinimalApi.Mappings;
using MinimalApi.Requests;
using MinimalApi.Responses;
using MinimalApi.Services;

namespace MeetupAPI.MinimalApi.Endpoints;

[HttpGet("api/v2/account/{id:int}"), AllowAnonymous]
public class GetUserEndpoint : Endpoint<GetUserDtoRequest, RegisterUserDtoResponse>
{
    private readonly IAccountService _accountService;

    public GetUserEndpoint(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public override async Task HandleAsync(GetUserDtoRequest request, CancellationToken ct)
    {
        var user = await _accountService.GetAsync(request.Id);

        if (user == null)
        {
            return;
        }

        var userResponse = user?.ToRegisterUserDtoResponse();
        await SendOkAsync(userResponse, ct);
    }
}
