using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using ErrorHandling.Exceptions;
using Meetup.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Controllers;

/// <summary>
/// GetMeetupRequest
/// </summary>
/// <param name="Name"></param>
public sealed record GetMeetupRequest(string Name);

/// <summary>
/// Simple Mediatr Abstraction
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IRequestHandler<TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// GetMeetupHandler
/// </summary>
public sealed class GetMeetupHandler : IRequestHandler<GetMeetupRequest, MeetupDetailsDto>
{
    private readonly MeetupContext _context;
    private readonly IMapper _mapper;

    public GetMeetupHandler(MeetupContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Handler for GetMeetupHandler
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ApiResponseException"></exception>
    public async Task<MeetupDetailsDto> Handle(GetMeetupRequest request, CancellationToken cancellationToken = default)
    {
        var meetup = await _context.Meetups
            .Include(m => m.Location)
            .Include(m => m.Lectures)
            .FirstOrDefaultAsync(m => m.Name.Replace(" ", "-").Equals(request.Name, System.StringComparison.CurrentCultureIgnoreCase),
                cancellationToken);

        if (meetup == null)
        {
            throw new ApiResponseException(HttpStatusCode.NotFound,
                $"Meetup with name {request.Name} has not been found");
        }

        return _mapper.Map<MeetupDetailsDto>(meetup);
    }
}

/// <summary>
/// Similar to pipeline behavior for loging a request in Mediatr
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public sealed class LoggingRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingRequestHandler<TRequest, TResponse>> _logger;
    private readonly IRequestHandler<TRequest, TResponse> _innerHandler;

    public LoggingRequestHandler(
        ILogger<LoggingRequestHandler<TRequest, TResponse>> logger,
        IRequestHandler<TRequest, TResponse> innerHandler)
    {
        _logger = logger;
        _innerHandler = innerHandler;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Begin pipeline behavior {Request}", request.GetType().Name);

        var response = await _innerHandler.Handle(request, cancellationToken);
        
        _logger.LogInformation("End pipeline behavior {Request}", request.GetType().Name);

        return response;
    }
}