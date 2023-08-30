using MediatR;

namespace MeetupAPI.Validation
{
    public interface IValidatableRequest<out TResponse> :
        IRequest<TResponse>, IValidatableRequest
    { }

    public interface IValidatableRequest { }
}
