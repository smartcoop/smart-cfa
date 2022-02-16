using AutoFixture;
using Core.Domain;
using Core.Domain.Enumerations;
using CSharpFunctionalExtensions;

namespace Smart.FA.Catalog.Tests.Common;

public class TrainerFactory
{
    public Trainer CreateClean()
    {
        var fixture = new Fixture();
        var defaultLanguage = Language.Create(fixture.Create<string>().Substring(0, 2));
        var name = Name.Create(fixture.Create<string>(), fixture.Create<string>());

        return new Trainer(name.Value,TrainerIdentity.Create(fixture.Create<string>(), ApplicationType.Account).Value,fixture.Create<string>(),fixture.Create<string>(), defaultLanguage.Value);
    }
    public Trainer Create(string firstName, string lastName)
    {
        var fixture = new Fixture();
        var defaultLanguage = Language.Create(fixture.Create<string>().Substring(0, 2));
        var name = Name.Create(firstName, lastName);
        return new Trainer(name.Value, TrainerIdentity.Create(fixture.Create<string>(), ApplicationType.Account).Value, fixture.Create<string>(), fixture.Create<string>(),defaultLanguage.Value );
    }

}
