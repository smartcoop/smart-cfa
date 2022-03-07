using AutoFixture;
using Core.Domain;
using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.SeedWork;
using CSharpFunctionalExtensions;

namespace Smart.FA.Catalog.Tests.Common;

public class TrainerFactory
{
    public Trainer CreateClean()
    {
        var fixture = new Fixture();
        var defaultLanguage = Language.Create(fixture.Create<string>().Substring(0, 2));
        var name = Name.Create(fixture.Create<string>(), fixture.Create<string>());

        return new Trainer
        (
            name.Value
            , TrainerIdentity.Create
            (
                fixture.Create<string>()
                , ApplicationType.Account
            ).Value
            , fixture.Create<string>()
            , fixture.Create<string>()
            , defaultLanguage.Value
        );
    }

    public Trainer Create(string firstName, string lastName)
    {
        var fixture = new Fixture();
        var defaultLanguage = Language.Create(fixture.Create<string>().Substring(0, 2));
        var name = Name.Create(firstName, lastName);
        return new Trainer
        (
            name.Value
            , TrainerIdentity.Create(fixture.Create<string>()
                , ApplicationType.Account).Value
            , fixture.Create<string>()
            , fixture.Create<string>(), defaultLanguage.Value);
    }

    public Trainer CreateFromUser(UserDto user)
    {
        var fixture = new Fixture();
        var defaultLanguage = Language.Create(fixture.Create<string>().Substring(0, 2));
        var name = Name.Create(user.FirstName, user.LastName);
        return new Trainer
        (name.Value
            , TrainerIdentity.Create
            (
                user.UserId
                , Enumeration.FromDisplayName<ApplicationType>(user.ApplicationType)
            ).Value
            , fixture.Create<string>()
            , fixture.Create<string>()
            , defaultLanguage.Value);
    }
}
