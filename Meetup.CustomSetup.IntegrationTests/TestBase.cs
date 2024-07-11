using AutoBogus;
using Xunit;

namespace Meetup.CustomSetup.IntegrationTests;

[Collection(nameof(TestFixture))]
public class TestBase : IDisposable
{
    public TestBase()
    {
        AutoFaker.Configure(builder =>
        {
            // configure global autobogus settings here
            builder.WithDateTimeKind(DateTimeKind.Utc)
                .WithRecursiveDepth(3)
                .WithTreeDepth(1)
                .WithRepeatCount(1);
        });
    }

    public void Dispose()
    {
    }
}
