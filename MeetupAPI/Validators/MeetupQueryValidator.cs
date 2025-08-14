using System.Linq;
using Entities;
using FluentValidation;
using MeetupAPI.Models;

namespace Validators;

public class MeetupQueryValidator : AbstractValidator<MeetupQuery>
{
    private readonly int[] _allowedPageSizes = [1, 5, 15, 50];
    private readonly string[] _allowedSortByColumnNames = [nameof(Meetup.Date), nameof(Meetup.Organizer), nameof(Meetup.Name)];
    
    public MeetupQueryValidator()
    {
        RuleFor(q => q.SearchPhrase).NotEmpty();
        RuleFor(q => q.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(q => q.PageSize).Custom((value, context) =>
        {
            if (!_allowedPageSizes.Contains(value))
            {
                context.AddFailure("PageSize", $"Page size must be in {string.Join(",", _allowedPageSizes)}");
            }
        });
        RuleFor(q => q.SortBy)
            .Must( value => string.IsNullOrEmpty(value) || _allowedSortByColumnNames.Contains(value) )
            .WithMessage($"SortBy is optional or it has to be in ({string.Join(",", _allowedSortByColumnNames)})");
    }
}
