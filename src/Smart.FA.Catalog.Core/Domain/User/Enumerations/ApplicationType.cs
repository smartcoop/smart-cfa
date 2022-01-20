using Core.SeedWork;

namespace Core.Domain.Enumerations;

public class ApplicationType : Enumeration
{
    public static ApplicationType Account = new(1, "Account");
    public static ApplicationType Default = new(0, "Default");

    public ApplicationType(int id, string name) : base(id, name)
    {
    }
}
