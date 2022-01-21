using System.Diagnostics.CodeAnalysis;

namespace Core.SeedWork;

public abstract class Entity
{
    #region Fields

    private List<DomainEvent> _domainEvents = new();

    #endregion

    #region Properties

    public virtual int Id { get; set; }
    public virtual bool IsTransient => Id == default;
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    #endregion

    #region Public Methods

    public void AddDomainEvent(DomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    #endregion

    #region Overrides

    public override bool Equals(object obj)
    {
        if (obj is not Entity entity)
            return false;

        if (ReferenceEquals(this, entity))
            return true;

        if (GetType() != entity.GetType())
            return false;

        if (entity.IsTransient || IsTransient)
            return false;

        return entity.Id == Id;
    }

    [SuppressMessage("ReSharper", "BaseObjectGetHashCodeCallInGetHashCode")]
    [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        if (IsTransient) return base.GetHashCode();

        return Id.GetHashCode() ^ 31;
    }

    #endregion

    #region Operators

    public static bool operator ==(Entity left, Entity right)
    {
        return left?.Equals(right) ?? Equals(right, null);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }

    #endregion

    #region Constructors

    protected Entity() { }

    #endregion
}
