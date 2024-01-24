using System;
using Xunit;

namespace Meetup.IntegrationTests;

public class CustomFactAttribute : FactAttribute // it could be changed to Theory if needed
{
    public CustomFactAttribute()
    {
        var stageVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    // set a name of a stage that needs to be skipped for a test
        Skip = stageVariable == "staging"
            ? "Test ignored"
            : Skip;
    }
}
