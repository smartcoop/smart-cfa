using Smart.FA.Catalog.UserAdmin.Domain.Domain;
using Smart.FA.Catalog.UserAdmin.Domain.Domain.Models;

namespace Smart.FA.Catalog.UserAdmin.Domain.Services;

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
    public CustomIdentity Identity { get; }

    /// <summary>
    /// <see cref="Trainer" /> instance of the current connected user.
    /// </summary>
    public Trainer CurrentTrainer { get; }

    /// <summary>
    /// Indicates if the connected user is a super admin.
    /// </summary>
    public bool IsSuperUser { get; }
}
