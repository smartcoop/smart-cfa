using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using static System.String;

namespace Core.SeedWork;

public abstract class Enumeration : IComparable
{
    #region Properties

    public string Name { get; }

    public int Id { get; }

    #endregion

    #region Constructor

    protected Enumeration()
    {
    }

    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    #endregion

    #region Overrides

    public override string ToString() => Name;

    public override bool Equals(object obj)
    {
        if (!(obj is Enumeration otherValue))
            return false;

        var typeMatches = GetType() == obj.GetType();
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    public override int GetHashCode() => Id.GetHashCode();

    #endregion

    #region Public Methods

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        return fields.Select(f => f.GetValue(null)).Cast<T>();
    }

    public static T FromValue<T>(int value) where T : Enumeration
    {
        var matchingItem = Parse<T, int>(value, "value", item => item.Id == value);
        return matchingItem;
    }

    public static List<T> FromValues<T>(IEnumerable<int> values) where T : Enumeration
        => values.Select(FromValue<T>).ToList();

    public static T FromDisplayName<T>(string displayName) where T : Enumeration
    {
        var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
        return matchingItem;
    }

    public static T FromDisplayNameInvariant<T>(string displayName) where T : Enumeration
    {
        if (IsNullOrWhiteSpace(displayName)) return null;
        var matchingItem = GetAll<T>().FirstOrDefault(item =>
            string.Equals(item.Name, displayName, StringComparison.InvariantCultureIgnoreCase));
        return matchingItem;
    }

    private static T Parse<T, TK>(TK value, string description, Func<T, bool> predicate) where T : Enumeration
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);

        if (matchingItem == null)
            throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

        return matchingItem;
    }

    public int CompareTo(object obj) => Id.CompareTo(((Enumeration) obj).Id);

    #endregion

    #region Operators

    public static bool operator ==(Enumeration a, Enumeration b)
    {
        if (ReferenceEquals(a, b))
            return true;

        if ((object) a == null || (object) b == null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Enumeration a, Enumeration b)
    {
        return !(a == b);
    }

    #endregion
}
