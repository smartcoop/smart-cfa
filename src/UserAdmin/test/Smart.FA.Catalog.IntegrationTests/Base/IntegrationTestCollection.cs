using Xunit;

namespace Smart.FA.Catalog.IntegrationTests.Base;

/// <summary>
/// A custom <see cref="ICollectionFixture{TFixture}"/> that will create an instance of the <see cref="TestSetup"/> class before a test class
/// <see cref="TestSetup"/> create a destroy and recreate a fresh database each time a test or a bulk (decorated by the collection fixture <see cref="IntegrationTestCollections.Default"/>)
/// of tests is run
/// </summary>
[CollectionDefinition(IntegrationTestCollections.Default)]
public class IntegrationTestCollection : ICollectionFixture<TestSetup>
{
}

public static class IntegrationTestCollections
{
    public const string Default = "Integration test collection";
}
