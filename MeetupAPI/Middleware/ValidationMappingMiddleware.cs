using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace MeetupAPI.Middleware
{
    public class ValidationMappingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException exception)
            {
                context.Response.StatusCode = 500;
                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Type = exception.GetType().ToString(),
                    Title = "Validation exception",
                    Detail = exception.Message
                };

                string json = JsonSerializer.Serialize(problem);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server error",
                    Title = "Server error",
                    Detail = "An internal server error has occured"
                };

                string json = JsonSerializer.Serialize(problem);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }
        }
    }
}
