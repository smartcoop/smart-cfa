using AutoFixture;
using Core.Domain;

namespace Smart.FA.Catalog.Tests.Common;

public class TrainerFactory
{
    public Trainer CreateClean()
    {
        var fixture = new Fixture();
        return new Trainer(fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>().Substring(0,2));
    }
    public Trainer Create(string firstName, string lastName)
    {
        var fixture = new Fixture();
        return new Trainer(firstName, lastName, fixture.Create<string>(), fixture.Create<string>().Substring(0,2));
    }

}
