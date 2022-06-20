using System.Security.Principal;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.Services;

namespace Smart.FA.Catalog.Core.Domain.Models;

public class CustomIdentity : IIdentity
{
    public int Id => Trainer.Id;

    public TrainerIdentity Type { get; set; }

    public Trainer Trainer { get; }

    public string AuthenticationType => "Custom";

    public bool IsAuthenticated => true;

    public string? Name { get; }

    public ConnectedUser? ConnectedUser { get; }

    public CustomIdentity(Trainer trainer, ConnectedUser? connectedUser)
    {
        Trainer = trainer;
        Name = GetUserFormattedName();
        ConnectedUser = connectedUser;
    }

    public override string ToString()
    {
        return GetUserFormattedName();
    }

    public string GetUserFormattedName() => $"{Trainer.Name.FirstName} {Trainer.Name.LastName}";
}

public record ConnectedUser(string UserId, string Email, Name Name);
