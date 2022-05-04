using System.Reflection;
using System.Runtime.CompilerServices;

namespace Smart.FA.Catalog.Shared.Domain.Enumerations.Common;

// This is a port of SmartEnum (https://github.com/ardalis/SmartEnum).

/// <summary>
/// A base type to use for creating enumerations with inner value of type <see cref="int"/>.
/// </summary>
/// <typeparam name="TEnum">The type that is inheriting from this class.</typeparam>
/// <remarks></remarks>
public abstract class Enumeration<TEnum> :
    Enumeration<TEnum, int>
    where TEnum : Enumeration<TEnum, int>
{
    protected Enumeration()
    {

    }

    protected Enumeration(int id, string name) :
        base(id, name)
    {
    }
}

/// <summary>
/// A base type to use for creating enumerations.
/// </summary>
/// <typeparam name="TEnum">The type that is inheriting from this class.</typeparam>
/// <typeparam name="TValue">The type of the inner value.</typeparam>
/// <remarks></remarks>
public abstract class Enumeration<TEnum, TValue> :
    IEnumeration,
    IEquatable<Enumeration<TEnum, TValue>>,
    IComparable<Enumeration<TEnum, TValue>>
    where TEnum : Enumeration<TEnum, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>
{
    static readonly Lazy<TEnum[]> _enumOptions =
        new Lazy<TEnum[]>(GetAllOptions, LazyThreadSafetyMode.ExecutionAndPublication);

    static readonly Lazy<Dictionary<string, TEnum>> _fromName =
        new Lazy<Dictionary<string, TEnum>>(() => _enumOptions.Value.ToDictionary(item => item.Name));

    static readonly Lazy<Dictionary<string, TEnum>> _fromNameIgnoreCase =
        new Lazy<Dictionary<string, TEnum>>(() => _enumOptions.Value.ToDictionary(item => item.Name, StringComparer.OrdinalIgnoreCase));

    static readonly Lazy<Dictionary<TValue, TEnum>> _fromValue =
        new Lazy<Dictionary<TValue, TEnum>>(() =>
        {
            // multiple enums with same value are allowed but store only one per value
            var dictionary = new Dictionary<TValue, TEnum>();
            foreach (var item in _enumOptions.Value)
            {
                if (!dictionary.ContainsKey(item._id))
                    dictionary.Add(item._id, item);
            }
            return dictionary;
        });

    private static TEnum[] GetAllOptions()
    {
        var baseType = typeof(TEnum);
        return Assembly.GetAssembly(baseType)!
            .GetTypes()
            .Where(t => baseType.IsAssignableFrom(t))
            .SelectMany(t => t.GetFieldsOfType<TEnum>())
            .OrderBy(t => t.Id)
            .ToArray();
    }

    /// <summary>
    /// Gets a collection containing all the instances of <see cref="Enumeration{TEnum,TValue}"/>.
    /// </summary>
    /// <value>A <see cref="IReadOnlyCollection{TEnum}"/> containing all the instances of <see cref="Enumeration{TEnum,TValue}"/>.</value>
    /// <remarks>Retrieves all the instances of <see cref="Enumeration{TEnum,TValue}"/> referenced by public static read-only fields in the current class or its bases.</remarks>
    public static IReadOnlyCollection<TEnum> List =>
        _fromName.Value.Values
            .ToList()
            .AsReadOnly();

    private readonly string _name;
    private readonly TValue _id;

    protected Enumeration()
    {
    }

    protected Enumeration(TValue? id, string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Argument cannot be null or empty.", nameof(name));
        if (id == null)
            ArgumentNullException.ThrowIfNull(id);

        _name = name;
        _id = id;
    }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>A <see cref="string"/> that is the name of the <see cref="Enumeration{TEnum,TValue}"/>.</value>
    public string Name => _name;

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>A <typeparamref name="TValue"/> that is the value of the <see cref="Enumeration{TEnum,TValue}"/>.</value>
    public TValue Id => _id;


    /// <summary>
    /// Gets the item associated with the specified name.
    /// </summary>
    /// <param name="name">The name of the item to get.</param>
    /// <param name="ignoreCase"><c>true</c> to ignore case during the comparison; otherwise, <c>false</c>.</param>
    /// <returns>
    /// The item associated with the specified name.
    /// If the specified name is not found, throws a <see cref="KeyNotFoundException"/>.
    /// </returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <c>null</c>.</exception>
    /// <exception cref="EnumerationNotFoundException"><paramref name="name"/> does not exist.</exception>
    /// <seealso cref="Enumeration{TEnum,TValue}.TryFromName(string, out TEnum)"/>
    /// <seealso cref="Enumeration{TEnum,TValue}.TryFromName(string, bool, out TEnum)"/>
    public static TEnum FromName(string name, bool ignoreCase = false)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Argument cannot be null or empty.", nameof(name));

        if (ignoreCase)
            return LocalFromName(_fromNameIgnoreCase.Value);
        else
            return LocalFromName(_fromName.Value);

        TEnum LocalFromName(Dictionary<string, TEnum> dictionary)
        {
            if (!dictionary.TryGetValue(name, out var result))
            {
                throw new EnumerationNotFoundException($"No {typeof(TEnum).Name} with Name \"{name}\" found.");
            }
            return result!;
        }
    }

    /// <summary>
    /// Gets the item associated with the specified name.
    /// </summary>
    /// <param name="name">The name of the item to get.</param>
    /// <param name="result">
    /// When this method returns, contains the item associated with the specified name, if the key is found;
    /// otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <c>true</c> if the <see cref="Enumeration{TEnum,TValue}"/> contains an item with the specified name; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <c>null</c>.</exception>
    /// <seealso cref="Enumeration{TEnum,TValue}.FromName(string, bool)"/>
    /// <seealso cref="Enumeration{TEnum,TValue}.TryFromName(string, bool, out TEnum)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFromName(string name, out TEnum result) =>
        TryFromName(name, false, out result);

    /// <summary>
    /// Gets the item associated with the specified name.
    /// </summary>
    /// <param name="name">The name of the item to get.</param>
    /// <param name="ignoreCase"><c>true</c> to ignore case during the comparison; otherwise, <c>false</c>.</param>
    /// <param name="result">
    /// When this method returns, contains the item associated with the specified name, if the name is found;
    /// otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <c>true</c> if the <see cref="Enumeration{TEnum,TValue}"/> contains an item with the specified name; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <c>null</c>.</exception>
    /// <seealso cref="Enumeration{TEnum,TValue}.FromName(string, bool)"/>
    /// <seealso cref="Enumeration{TEnum,TValue}.TryFromName(string, out TEnum)"/>
    public static bool TryFromName(string name, bool ignoreCase, out TEnum result)
    {
        if (string.IsNullOrEmpty(name))
        {
            result = default;
            return false;
        }

        if (ignoreCase)
            return _fromNameIgnoreCase.Value.TryGetValue(name, out result);
        else
            return _fromName.Value.TryGetValue(name, out result);
    }

    /// <summary>
    /// Gets an item associated with the specified value.
    /// </summary>
    /// <param name="value">The value of the item to get.</param>
    /// <returns>
    /// The first item found that is associated with the specified value.
    /// If the specified value is not found, throws a <see cref="KeyNotFoundException"/>.
    /// </returns>
    /// <exception cref="EnumerationNotFoundException"><paramref name="value"/> does not exist.</exception>
    /// <seealso cref="Enumeration{TEnum,TValue}.FromValue(TValue, TEnum)"/>
    /// <seealso cref="Enumeration{TEnum,TValue}.TryFromValue(TValue, out TEnum)"/>
    public static TEnum FromValue(TValue value)
    {
        if (value == null)
            ArgumentNullException.ThrowIfNull(value);

        if (!_fromValue.Value.TryGetValue(value, out var result))
        {
            throw new EnumerationNotFoundException($"No {typeof(TEnum).Name} with Value {value} found.");
        }
        return result;
    }

    /// <summary>
    /// Gets an item associated with the specified value.
    /// </summary>
    /// <param name="value">The value of the item to get.</param>
    /// <param name="defaultValue">The value to return when item not found.</param>
    /// <returns>
    /// The first item found that is associated with the specified value.
    /// If the specified value is not found, returns <paramref name="defaultValue"/>.
    /// </returns>
    /// <seealso cref="Enumeration{TEnum,TValue}.FromValue(TValue)"/>
    /// <seealso cref="Enumeration{TEnum,TValue}.TryFromValue(TValue, out TEnum)"/>
    public static TEnum FromValue(TValue? value, TEnum defaultValue)
    {
        if (value == null)
            ArgumentNullException.ThrowIfNull(value);

        if (!_fromValue.Value.TryGetValue(value, out var result))
        {
            return defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Gets an item associated with the specified value.
    /// </summary>
    /// <param name="value">The value of the item to get.</param>
    /// <param name="result">
    /// When this method returns, contains the item associated with the specified value, if the value is found;
    /// otherwise, <c>null</c>. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <c>true</c> if the <see cref="Enumeration{TEnum,TValue}"/> contains an item with the specified name; otherwise, <c>false</c>.
    /// </returns>
    /// <seealso cref="Enumeration{TEnum,TValue}.FromValue(TValue)"/>
    /// <seealso cref="Enumeration{TEnum,TValue}.FromValue(TValue, TEnum)"/>
    public static bool TryFromValue(TValue? value, out TEnum? result)
    {
        if (value == null)
        {
            result = default;
            return false;
        }

        return _fromValue.Value.TryGetValue(value, out result);
    }

    public static List<TEnum> FromValues(IEnumerable<TValue>? ids)
    {
        if (ids == null)
            ArgumentNullException.ThrowIfNull(ids);

        return ids.Select(FromValue).ToList();
    }

    public override string ToString() =>
        _name;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() =>
        _id.GetHashCode();

    public override bool Equals(object obj) =>
        (obj is Enumeration<TEnum, TValue> other) && Equals(other);

    /// <summary>
    /// Returns a value indicating whether this instance is equal to a specified <see cref="Enumeration{TEnum,TValue}"/> value.
    /// </summary>
    /// <param name="other">An <see cref="Enumeration{TEnum,TValue}"/> value to compare to this instance.</param>
    /// <returns><c>true</c> if <paramref name="other"/> has the same value as this instance; otherwise, <c>false</c>.</returns>
    public virtual bool Equals(Enumeration<TEnum, TValue>? other)
    {
        // check if same instance
        if (ReferenceEquals(this, other))
            return true;

        // it's not same instance so
        // check if it's not null and is same value
        if (other is null)
            return false;

        return _id.Equals(other._id);
    }

    public static bool operator ==(Enumeration<TEnum, TValue>? left, Enumeration<TEnum, TValue>? right)
    {
        // Handle null on left side
        if (left is null)
            return right is null; // null == null = true

        // Equals handles null on right side
        return left.Equals(right);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Enumeration<TEnum, TValue> left, Enumeration<TEnum, TValue> right) =>
        !(left == right);

    /// <summary>
    /// Compares this instance to a specified <see cref="Enumeration{TEnum,TValue}"/> and returns an indication of their relative values.
    /// </summary>
    /// <param name="other">An <see cref="Enumeration{TEnum,TValue}"/> value to compare to this instance.</param>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="other"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public virtual int CompareTo(Enumeration<TEnum, TValue>? other) =>
        _id.CompareTo(other._id);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <(Enumeration<TEnum, TValue> left, Enumeration<TEnum, TValue> right) =>
        left.CompareTo(right) < 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator <=(Enumeration<TEnum, TValue> left, Enumeration<TEnum, TValue> right) =>
        left.CompareTo(right) <= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >(Enumeration<TEnum, TValue> left, Enumeration<TEnum, TValue> right) =>
        left.CompareTo(right) > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator >=(Enumeration<TEnum, TValue> left, Enumeration<TEnum, TValue> right) =>
        left.CompareTo(right) >= 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator TValue?(Enumeration<TEnum, TValue>? enumeration) =>
        enumeration is not null
            ? enumeration!._id!
            : default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Enumeration<TEnum, TValue>(TValue value) =>
        FromValue(value);
}
