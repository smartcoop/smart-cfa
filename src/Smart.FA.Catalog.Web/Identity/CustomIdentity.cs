using System.Security.Principal;
using Core.Domain;

namespace Api.Identity;

public class CustomIdentity : IIdentity
{
    public Trainer Trainer { get; }


    public string AuthenticationType => "Custom";

    public bool IsAuthenticated => true;
    public string? Name { get; }

    public CustomIdentity(Trainer trainer)
    {
        Trainer = trainer;
        Name = GetUserData();
    }

    public override string ToString()
    {
        return GetUserData();
    }

    public string GetUserData()
        => $"{Trainer.Name.FirstName} {Trainer.Name.LastName}";
}
