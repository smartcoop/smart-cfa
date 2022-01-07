using Core.SeedWork;
using CSharpFunctionalExtensions;
using ValueObject = CSharpFunctionalExtensions.ValueObject;

namespace Core.Domain;

public class Name: ValueObject
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    private Name(string firstName, string lastName):this()
    {
        FirstName = firstName;
        LastName = lastName;
    }

    protected Name()
    {

    }

    public static Result<Name> Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<Name>("First name should not be empty");
        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure<Name>("Last name should not be empty");
        if(firstName.Length > 200)
            return Result.Failure<Name>("First name is too long");
        if(firstName.Length > 200)
            return Result.Failure<Name>("Last name is too long");

        return Result.Success(new Name(firstName, lastName));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}
