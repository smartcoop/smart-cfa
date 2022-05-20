using System.Security.Principal;

namespace Smart.FA.Catalog.UserAdmin.Domain.Domain.Models;

public class CustomIdentity : IIdentity
{
    public int Id => Trainer.Id;

    public Trainer Trainer { get; }

    public string AuthenticationType => "Custom";

    public bool IsAuthenticated => true;

    public string? Name { get; }

    public CustomIdentity(Trainer trainer)
    {
        Trainer = trainer;
        Name    = GetUserFormattedName();
    }

    public override string ToString()
    {
        return GetUserFormattedName();
    }

    public string GetUserFormattedName() => $"{Trainer.Name.FirstName} {Trainer.Name.LastName}";
}
