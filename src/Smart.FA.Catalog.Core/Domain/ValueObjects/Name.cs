using Core.Exceptions;
using Core.SeedWork;
using CSharpFunctionalExtensions;
using ValueObject = CSharpFunctionalExtensions.ValueObject;

namespace Core.Domain;

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
}
