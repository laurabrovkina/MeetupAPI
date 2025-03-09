using System.Linq;
using System.Net;
using ErrorHandling.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ErrorHandling;

public static class ErrorMessages
{
    public static void BadRequestMessage<T>(T model, ModelStateDictionary modelState)
    {
        var errors = modelState.Select(x => x.Value.Errors)
            .Where(y => y.Count > 0)
            .ToList();

        throw new ApiResponseException(HttpStatusCode.BadRequest, $"There are {errors.Count} validation errors in {model}");
    }
}