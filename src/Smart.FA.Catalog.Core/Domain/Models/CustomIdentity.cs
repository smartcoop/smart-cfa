using System.Security.Principal;

namespace Core.Domain.Models;

public class CustomIdentity : IIdentity
{
    public int Id { get; set; }

    public Trainer Trainer { get; }

    public string AuthenticationType => "Custom";

    public bool IsAuthenticated => true;

    public string? Name { get; }

    public CustomIdentity(Trainer trainer)
    {
        Trainer = trainer;
        Id      = trainer.Id;
        Name    = GetUserData();
    }

    public override string ToString()
    {
        return GetUserData();
    }

    public string GetUserData() => $"{Trainer.Name.FirstName} {Trainer.Name.LastName}";
}
