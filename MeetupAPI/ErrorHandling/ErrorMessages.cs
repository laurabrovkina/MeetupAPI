using MeetupAPI.ErrorHandling.Exceptions;
using MeetupAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Net;

public static class ErrorMessages
{
    public static void BadRequestMessage(object model, ModelStateDictionary modelState)
    {
        var errors = modelState.Select(x => x.Value.Errors)
        .Where(y => y.Count > 0)
        .ToList();

        throw new ApiResponseException(HttpStatusCode.BadRequest, $"There are {errors.Count} validation errors in {model}");
    }
}