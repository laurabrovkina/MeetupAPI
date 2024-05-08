using MeetupAPI.ErrorHandling.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace MeetupAPI.ErrorHandling;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public ExceptionHandlingMiddleware(RequestDelegate requestDelegate,
        ProblemDetailsFactory problemDetailsFactory)
    {
        _next = requestDelegate;
        _problemDetailsFactory = problemDetailsFactory;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch(ApiResponseException ex)
        {
            await HandleExceptions(httpContext, (int)ex.StatusCode, ex.Data, ex.Message);
        }
        catch (Exception ex)
        {
            await HandleExceptions(httpContext,StatusCodes.Status500InternalServerError, ex.Data);
        }
    }

    private async Task HandleExceptions(HttpContext httpContext, int statusCode, IDictionary additionalData, string errorMessage = null)
    {
        var problemDetails = _problemDetailsFactory.CreateProblemDetails(httpContext,
            statusCode: statusCode,
            instance: httpContext.Request.Path,
            detail: errorMessage);

        AddVersionAndRequestMinimumInformationToExtensions(problemDetails);

        await httpContext.Response.WriteAsJsonAsync(problemDetails, options: null, "application/problem+json");
    }

    // Examples what could be included in extensions
    private static void AddVersionAndRequestMinimumInformationToExtensions(ProblemDetails problemDetails)
    {
        problemDetails.Extensions["ServiceVersion"] = "1.0";
        problemDetails.Extensions["ClientRequestId"] = Guid.NewGuid().ToString();
    }

    private static void AddAdditionalDataToProblemDetail(ProblemDetails problemDetails, IDictionary additionalData)
    {
        foreach (var item in additionalData.Keys)
        {
            var stringKey = item.ToString();
            if (stringKey is not null)
            {
                problemDetails.Extensions[stringKey] = additionalData[item];
            }
        }
    }
}