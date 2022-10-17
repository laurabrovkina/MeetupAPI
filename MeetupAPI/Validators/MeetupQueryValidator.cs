using System.Linq;
using FluentValidation;
using MeetupAPI.Models;

namespace MeetupAPI.Validators
{
    public class MeetupQueryValidator : AbstractValidator<MeetupQuery>
    {
        private int[] allowedPageSizes = new[] { 1, 5, 15, 50 };
        public MeetupQueryValidator()
        {
            RuleFor(q => q.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(q => q.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"Page size must be in {string.Join(",", allowedPageSizes)}");
                }
            });
        }
    }
}
