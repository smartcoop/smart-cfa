namespace Smart.FA.Catalog.Core.Domain;

/// <summary>
/// Add Soft Delete field to an entity
/// </summary>
public interface IEFSoftDelete
{
    public bool IsDeleted { get; }
    public bool IsDestroyed { get; }
    /// <summary>
    /// Soft Delete the entity.
    /// The entity could be retrieved later on.
    /// </summary>
    public void Delete();
    /// <summary>
    /// Hard Delete the entity.
    /// The entity is removed from database.
    /// </summary>
    public void Destroy();
}
