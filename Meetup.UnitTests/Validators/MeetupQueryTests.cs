using System.Collections;
using System.Collections.Generic;
using FluentValidation.TestHelper;
using MeetupAPI.Models;
using MeetupAPI.Validators;
using Xunit;

namespace Meetup.UnitTests.Validators;

public class MeetupQueryTests
{
    private readonly MeetupQueryValidator _validator = new();

    [Fact]
    public void Should_Not_Have_Validation_Error_For_Valid_Page_Size()
    {
        var sut = new MeetupQuery
        {
            SearchPhrase = "meetup",
            PageSize = 1,
            PageNumber = 1
        };

        var result = _validator.TestValidate(sut);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(50)]
    public void Should_Not_Have_Validation_Error_For_Valid_Number_Of_Pages(int allowedPageSizes)
    {
        var sut = new MeetupQuery
        {
            SearchPhrase = "meetup",
            PageSize = allowedPageSizes,
            PageNumber = 1
        };

        var result = _validator.TestValidate(sut);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [ClassData(typeof(ValidSortByNameColumnNames))]
    public void Should_Not_Have_Validation_Error_For_Valid_SortBy_Column_Names(string allowedSortByColumnNames)
    {
        var sut = new MeetupQuery
        {
            SearchPhrase = "meetup",
            PageSize = 1,
            PageNumber = 1,
            SortBy = allowedSortByColumnNames
        };

        var result = _validator.TestValidate(sut);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Throw_Error_When_Page_Number_Is_Not_Valid()
    {
        var sut = new MeetupQuery
        {
            SearchPhrase = "meetup",
            PageSize = 1,
            PageNumber = 0
        };

        var result = _validator.TestValidate(sut);
        result.ShouldHaveValidationErrorFor(x => x.PageNumber);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(-1)]
    [InlineData(1000)]
    public void Should_Throw_Error_When_Page_Size_Is_Not_Valid(int pageSize)
    {
        var sut = new MeetupQuery
        {
            SearchPhrase = "meetup",
            PageSize = pageSize,
            PageNumber = 1
        };

        var result = _validator.TestValidate(sut);
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Throw_Error_When_Search_Phrase_Is_Null_Or_Empty(string searchPhrase)
    {
        // Arrange
        var sut = new MeetupQuery
        {
            SearchPhrase = searchPhrase,
            PageSize = 1,
            PageNumber = 1
        };

        // Act
        var result = _validator.TestValidate(sut);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SearchPhrase);
    }

    [Theory]
    [MemberData(nameof(InvalidSortByColumnNames))]
    public void Should_Throw_Error_For_Invalid_SortBy_Column_Names(string invalidSortByColumnNames)
    {
        var allowedSortByColumnNames = new[]
        {
            nameof(MeetupAPI.Entities.Meetup.Date),
            nameof(MeetupAPI.Entities.Meetup.Organizer),
            nameof(MeetupAPI.Entities.Meetup.Name)
        };

        var sut = new MeetupQuery
        {
            SearchPhrase = "meetup",
            PageSize = 1,
            PageNumber = 1,
            SortBy = invalidSortByColumnNames
        };

        var result = _validator.TestValidate(sut);
        result.ShouldHaveValidationErrorFor(x => x.SortBy)
            .WithErrorMessage($"SortBy is optional or it has to be in ({string.Join(",", allowedSortByColumnNames)})");
    }

    public static IEnumerable<object[]> InvalidSortByColumnNames()
    {
        yield return ["InvalidColumnName"];
        yield return [nameof(MeetupAPI.Entities.Meetup.IsPrivate)];
        yield return [nameof(MeetupAPI.Entities.Meetup.Location)];
        yield return [nameof(MeetupAPI.Entities.Meetup.Lectures)];
        yield return [nameof(MeetupAPI.Entities.Meetup.Id)];
        yield return [nameof(MeetupAPI.Entities.Meetup.CreatedBy)];
        yield return [nameof(MeetupAPI.Entities.Meetup.CreatedById)];
    }
}

public class ValidSortByNameColumnNames : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [nameof(MeetupAPI.Entities.Meetup.Date)];
        yield return [nameof(MeetupAPI.Entities.Meetup.Organizer)];
        yield return [nameof(MeetupAPI.Entities.Meetup.Name)];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}