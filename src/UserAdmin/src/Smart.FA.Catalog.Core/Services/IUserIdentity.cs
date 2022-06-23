using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Models;

namespace Smart.FA.Catalog.Core.Services;

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

    /// <summary>
    /// Indicates if the connected user is a social member (the opposite would be a permanent member).
    /// </summary>
    public bool IsShareholder { get; }
}
