using AutoFixture;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.User.Dto;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;
using Smart.FA.Catalog.Core.Domain.ValueObjects;

namespace Smart.FA.Catalog.Tests.Common.Factories;

public static class MockedTrainerFactory
{
    private static Fixture fixture = new();
    public static Trainer CreateClean()
    {
        var defaultLanguage = Language.Create(fixture.Create<string>()[..2]);
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
            , $"{Guid.NewGuid()}@gmail.com"
        );
    }

    public static Trainer Create(string firstName, string lastName)
    {
        var fixture = new Fixture();
        var defaultLanguage = Language.Create(fixture.Create<string>()[..2]);
        var name = Name.Create(firstName, lastName);
        return new Trainer
        (
            name.Value
            , TrainerIdentity.Create(fixture.Create<string>()
                , ApplicationType.Account).Value
            , fixture.Create<string>()
            , fixture.Create<string>(), defaultLanguage.Value
            , $"{Guid.NewGuid()}@gmail.com"
            );
    }

    public static Trainer CreateFromUser(UserDto user)
    {
        var fixture = new Fixture();
        var defaultLanguage = Language.Create(fixture.Create<string>().Substring(0, 2));
        var name = Name.Create(user.FirstName, user.LastName);
        return new Trainer
        (name.Value
            , TrainerIdentity.Create
            (
                user.UserId
                , ApplicationType.FromName(user.ApplicationType)
            ).Value
            , fixture.Create<string>()
            , fixture.Create<string>()
            , defaultLanguage.Value
            , $"{Guid.NewGuid()}@gmail.com"
            );
    }
}
