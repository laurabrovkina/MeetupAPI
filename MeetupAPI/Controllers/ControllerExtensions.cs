using FluentValidation;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace MeetupAPI.Controllers
{
    public static class ControllerExtensions
    {
        public static IActionResult ToOk<TResult>(
            this Result<TResult> result)
        {
            return result.Match<ActionResult>(obj =>
            {
                return new OkObjectResult(result);
            }, exception =>
            {
                if (exception is ValidationException validationException)
                {
                    return new BadRequestObjectResult(validationException);
                }

                return new StatusCodeResult(500);
            });
        }
    }
}
