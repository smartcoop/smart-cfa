using Smart.FA.Catalog.Shared.Domain.Enumerations.Common;

namespace Smart.FA.Catalog.UserAdmin.Domain.Domain.User.Enumerations;

public class ApplicationType : Enumeration<ApplicationType>
{
    public static ApplicationType Account = new(1, "Account");
    public static ApplicationType Default = new(0, "Default");

    public ApplicationType(int id, string name) : base(id, name)
    {
    }
}
