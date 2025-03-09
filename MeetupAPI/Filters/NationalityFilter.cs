using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Filters;

public class NationalityFilter : Attribute, IAuthorizationFilter
{
    private readonly string[] _nationalities;

    public NationalityFilter(string nationalities)
    {
        _nationalities = nationalities.Split(",");
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var nationality = context.HttpContext.User.FindFirst(c => c.Type == "Nationality").Value;

        if (!_nationalities.Any(c => c == nationality))
        {
            context.Result = new StatusCodeResult(403);
        }
    }
}
