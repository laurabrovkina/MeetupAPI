﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MeetupAPI.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }
#if DEBUG
    public void OnException(ExceptionContext context)
    {
        _logger.LogCritical($"Caught in ExceptionFilter {context.Exception.Message}", context.Exception);

        var result = new JsonResult("Something went wrong");
        result.StatusCode = 500;

        context.Result = result;
    }
}
#endif
