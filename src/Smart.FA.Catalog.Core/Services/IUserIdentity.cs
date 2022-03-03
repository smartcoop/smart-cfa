using Core.Domain;
using Core.Domain.Models;

namespace Core.Services;

/// <summary>
/// Exposes the <see cref="CustomIdentity" /> of the current connected user.
/// </summary>
public interface IUserIdentity
{
    /// <summary>
    /// Id of the current user.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// The identity of the connected user.
    /// </summary>
    public CustomIdentity Identity { get; init; }

    /// <summary>
    /// <see cref="Trainer" /> instance of the current connected user.
    /// </summary>
    public Trainer CurrentTrainer { get; init; }
}
