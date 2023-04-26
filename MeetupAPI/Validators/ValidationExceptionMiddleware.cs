using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MeetupAPI.Validators
{
    public class ValidationExceptionMiddleware
    {
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public ValidationExceptionMiddleware(ProblemDetailsFactory problemDetailsFactory)
        {
            _problemDetailsFactory = problemDetailsFactory;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException exception)
            {
                // set http status code and content type
                var problem = _problemDetailsFactory.CreateProblemDetails(
                    context,
                    statusCode: StatusCodes.Status400BadRequest,
                    detail: exception.Message,
                    title: "Bad Request",
                    type: $"{exception.GetType()}");

                // writes / returns error model to the response
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(problem));
            }
        }
    }
}
