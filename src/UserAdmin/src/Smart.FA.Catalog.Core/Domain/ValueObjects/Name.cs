using CSharpFunctionalExtensions;
using Smart.FA.Catalog.Core.Exceptions;
using ValueObject = CSharpFunctionalExtensions.ValueObject;

namespace Smart.FA.Catalog.Core.Domain.ValueObjects;

public class Name: ValueObject
{
    public string FirstName { get; } = null!;
    public string LastName { get; }= null!;

    private Name(string firstName, string lastName):base()
    {
        FirstName = firstName;
        LastName = lastName;
    }

    protected Name() { }

    public static Result<Name, Error> Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Errors.General.ValueIsRequired();
        if (string.IsNullOrWhiteSpace(lastName))
            return Errors.General.ValueIsRequired();
        if (firstName.Length > 200)
            return Errors.General.InvalidLength();
        if(firstName.Length > 200)
            return Errors.General.InvalidLength();

        return new Name(firstName, lastName);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }

    public override string ToString()
    {
        return FirstName + " " + LastName;
    }

    public static implicit operator string(Name name) => name.ToString();
}
