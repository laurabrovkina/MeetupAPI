using FluentValidation;
using MediatR.Pipeline;
using System.Threading;
using System.Threading.Tasks;

namespace MeetupAPI.Validation
{
    public class ValidationProcessor<TRequest> : IRequestPreProcessor<TRequest>
        where TRequest : IValidatableRequest
    {
        private readonly IValidator<TRequest> _validator;

        public ValidationProcessor(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);
        }
    }
}
