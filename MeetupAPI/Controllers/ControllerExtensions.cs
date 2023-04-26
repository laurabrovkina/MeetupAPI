using FluentValidation;
using LanguageExt.Common;
using MeetupAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MeetupAPI.Controllers
{
    public static class ControllerExtensions
    {
        public static ActionResult ToOk<TResult, TContract>(
            this Result<TResult> result,TContract contract)
        {
            return result.Match<ActionResult>(obj =>
            {
                return new OkObjectResult(contract);
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
