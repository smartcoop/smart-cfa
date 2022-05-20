using System.Diagnostics.CodeAnalysis;

namespace Smart.FA.Catalog.UserAdmin.Domain.SeedWork;

public abstract class ValueObject
{
    #region Public Methods

    protected abstract IEnumerable<object> GetAtomicValues();

    public ValueObject? GetCopy()
    {
        return MemberwiseClone() as ValueObject;
    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    protected static bool EqualOperator(ValueObject? left, ValueObject? right)
    {
        if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            return false;

        return ReferenceEquals(left, null) || left.Equals(right);
    }

    protected static bool NotEqualOperator(ValueObject? left, ValueObject right)
    {
        return !EqualOperator(left, right);
    }

    #endregion

    #region Overrides

    [SuppressMessage("ReSharper", "GenericEnumeratorNotDisposed")]
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;
        var thisValues = GetAtomicValues().GetEnumerator();
        var otherValues = other.GetAtomicValues().GetEnumerator();

        while (thisValues.MoveNext() && otherValues.MoveNext())
        {
            if (ReferenceEquals(thisValues.Current, null) ^ ReferenceEquals(otherValues.Current, null))
                return false;

            if (thisValues.Current != null && !thisValues.Current.Equals(otherValues.Current))
                return false;
        }

        return !thisValues.MoveNext() && !otherValues.MoveNext();
    }

    public override int GetHashCode()
    {
        return GetAtomicValues()
              .Select(x => x.GetHashCode())
              .Aggregate((x, y) => x ^ y);
    }

    #endregion

    #region Operators

    public static bool operator ==(ValueObject? a, ValueObject? b)
    {
        if (ReferenceEquals(a, b))
            return true;
        if (a is null || b is null)
            return false;
        return a.Equals(b);
    }

    public static bool operator !=(ValueObject a, ValueObject b)
    {
        return !(a == b);
    }

    #endregion
}
