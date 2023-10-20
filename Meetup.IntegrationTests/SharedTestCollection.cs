using Xunit;

namespace Meetup.IntegrationTests;

[CollectionDefinition("Test collection")]
public class SharedTestCollection : ICollectionFixture<TestFixture>
{

}
