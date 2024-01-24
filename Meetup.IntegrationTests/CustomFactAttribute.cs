using System;
using Xunit;

namespace Meetup.IntegrationTests;

public class CustomFactAttribute : FactAttribute
{
    public CustomFactAttribute()
    {
        var stageVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        Skip = stageVariable == "staging"
            ? "Test ignored"
            : Skip;
    }
}
