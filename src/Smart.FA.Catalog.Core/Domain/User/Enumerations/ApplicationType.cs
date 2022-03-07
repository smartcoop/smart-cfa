using Core.SeedWork;

namespace Core.Domain.Enumerations;

public class ApplicationType : Enumeration
{
    public static ApplicationType Account = new(1, "Account");

    public ApplicationType(int id, string name) : base(id, name)
    {
    }
}
