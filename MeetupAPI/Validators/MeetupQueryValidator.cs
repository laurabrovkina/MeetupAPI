﻿using System.Linq;
using FluentValidation;
using MeetupAPI.Entities;
using MeetupAPI.Models;

namespace MeetupAPI.Validators;

public class MeetupQueryValidator : AbstractValidator<MeetupQuery>
{
    private int[] allowedPageSizes = new[] { 1, 5, 15, 50 };
    private string[] allowedSortByColumnNames = { nameof(Meetup.Date), nameof(Meetup.Organizer), nameof(Meetup.Name) };
    public MeetupQueryValidator()
    {
        RuleFor(q => q.SearchPhrase).NotEmpty();
        RuleFor(q => q.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(q => q.PageSize).Custom((value, context) =>
        {
            if (!allowedPageSizes.Contains(value))
            {
                context.AddFailure("PageSize", $"Page size must be in {string.Join(",", allowedPageSizes)}");
            }
        });
        RuleFor(q => q.SortBy)
            .Must( value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value) )
            .WithMessage($"SortBy is optional or it has to be in ({string.Join(",", allowedSortByColumnNames)})");
    }
}
