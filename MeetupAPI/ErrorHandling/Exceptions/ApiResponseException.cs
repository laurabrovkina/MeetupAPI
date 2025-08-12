using System;
using System.Net;

namespace ErrorHandling.Exceptions;

public class ApiResponseException : Exception
{
    public ApiResponseException(HttpStatusCode httpStatusCode, string message) : base(message)
    {
        StatusCode = httpStatusCode;
    }

    public HttpStatusCode StatusCode { get; }
}