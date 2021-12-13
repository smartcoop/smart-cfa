using Xunit;

namespace Smart.FA.Catalog.IntegrationTests.Base;

[CollectionDefinition("Integration test collection")]

public class IntegrationTestCollection : ICollectionFixture<TestSetup>
{
}
