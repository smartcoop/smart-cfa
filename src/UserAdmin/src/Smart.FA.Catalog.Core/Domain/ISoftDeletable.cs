namespace Smart.FA.Catalog.Core.Domain;

/// <summary>
/// Add Soft Delete field to an entity
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// The field marks the entity as soft deleted, meaning it could be recovered in the database.
    /// </summary>
    public DateTime? SoftDeletedAt { get; }
}
