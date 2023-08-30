using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MeetupAPI.Middleware
{
    public class ValidationMappingMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMappingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException exception)
            {
                context.Response.StatusCode = 400;

                var result = JsonConvert.SerializeObject(new { Error = exception.Message });

                await context.Response.WriteAsync(result);
            }
            catch (Exception exception)
            {
                context.Response.StatusCode = 400;

                var result = JsonConvert.SerializeObject(new { Error = exception.Message });

                await context.Response.WriteAsync(result);
            }
        }

        //public void Invoke(HttpContext context)
        //{
        //    try
        //    {
        //        _next(context);
        //    }
        //    catch (ValidationException exception)
        //    {
        //        context.Response.StatusCode = 400;

        //        var result = JsonConvert.SerializeObject(new { Error = exception.Message }, Formatting.None);

        //        context.Response.WriteAsync(result);
        //    }
        //}
    }
}
