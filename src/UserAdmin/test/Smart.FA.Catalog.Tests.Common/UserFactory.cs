using System.Globalization;
using AutoFixture;
using Smart.FA.Catalog.Core.Domain.User.Dto;
using Smart.FA.Catalog.Core.Domain.User.Enumerations;

namespace Smart.FA.Catalog.Tests.Common;

public static class UserFactory
{
    private static Fixture fixture = new();

    public static UserDto CreateClean()
    {
        return new UserDto
        (
            fixture.Create<int>()
                .ToString(CultureInfo.CurrentCulture)
            , fixture.Create<string>(),
            fixture.Create<string>()
            , ApplicationType.Account.Name, "victor@victor.com");
    }
}
