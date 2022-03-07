using System.Globalization;
using AutoFixture;
using Core.Domain.Dto;
using Core.Domain.Enumerations;

namespace Smart.FA.Catalog.Tests.Common;

public class UserFactory
{
    private Fixture fixture = new();

    public UserDto CreateClean()
    {
        return new UserDto
        (
            fixture.Create<int>()
                .ToString(CultureInfo.CurrentCulture)
            , fixture.Create<string>(),
            fixture.Create<string>()
            , ApplicationType.Account.Name
        );
    }
}
