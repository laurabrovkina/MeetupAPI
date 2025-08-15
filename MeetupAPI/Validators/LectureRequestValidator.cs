using FluentValidation;
using MyNamespace;

namespace Validators;

public class LectureRequestValidator : AbstractValidator<LectureRequest>
{
    public LectureRequestValidator()
    {
        RuleFor(x => x.Author)
            .NotEmpty()
            .WithMessage("Author is required")
            .MinimumLength(5)
            .WithMessage("Author should be at least 5 characters long");
        
        RuleFor(x => x.Topic)
            .NotEmpty()
            .WithMessage("Topic is required")
            .MinimumLength(5)
            .WithMessage("Topic should be at least 5 characters long");
    }
}