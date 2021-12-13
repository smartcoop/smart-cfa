using System.Diagnostics.CodeAnalysis;

namespace Core.SeedWork;

public abstract class ValueObject
{
    #region Public Methods

    protected abstract IEnumerable<object> GetAtomicValues();

    public ValueObject GetCopy()
    {
        return MemberwiseClone() as ValueObject;
    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    protected static bool EqualOperator(ValueObject left, ValueObject right)
    {
        if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            return false;

        return ReferenceEquals(left, null) || left.Equals(right);
    }

    protected static bool NotEqualOperator(ValueObject left, ValueObject right)
    {
        return !EqualOperator(left, right);
    }

    #endregion

    #region Overrides

    [SuppressMessage("ReSharper", "GenericEnumeratorNotDisposed")]
    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType())
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
              .Select(x => x != null ? x.GetHashCode() : 0)
              .Aggregate((x, y) => x ^ y);
    }

    #endregion

    #region Operators

    public static bool operator ==(ValueObject a, ValueObject b)
    {
        if (ReferenceEquals(a, b))
            return true;

        if ((object)a == null || (object)b == null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject a, ValueObject b)
    {
        return !(a == b);
    }

    #endregion
}
