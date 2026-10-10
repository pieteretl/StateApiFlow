using Xunit;

namespace StateFlowApi.Tests.TestFixtures;

[CollectionDefinition("AWS Collection")]
public class TestCollectionFixture : ICollectionFixture<AwsFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to define the collection that other test classes can be part of,
    // using the [Collection("AWS Collection")] attribute.
}

