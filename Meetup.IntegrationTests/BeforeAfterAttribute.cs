using System;
using System.Reflection;
using Xunit.Sdk;

namespace Meetup.IntegrationTests;

public class BeforeAfterAttribute : BeforeAfterTestAttribute
{
    public override void Before(MethodInfo methodUnderTest)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
    }
    
    public override void After(MethodInfo methodUnderTest)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
    }
    
}