using AutoFixture;
using Core.Domain;

namespace Smart.FA.Catalog.Tests.Common;

public class TrainerFactory
{
    public Trainer Create()
    {
        var fixture = new Fixture();
        return new Trainer(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>());
    }
}
