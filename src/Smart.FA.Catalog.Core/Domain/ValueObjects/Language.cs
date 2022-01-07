using CSharpFunctionalExtensions;

namespace Core.Domain;

public class Language: ValueObject
{
    public string Value { get; }
    private Language(string value)
    {
        Value = value;
    }

    public static Result<Language> Create(string language)
    {
        if (string.IsNullOrWhiteSpace(language))
            return Result.Failure<Language>("Language should not be empty");

        language = language.Trim();
        language = language.ToUpperInvariant();

        if (language.Length != 2)
            return Result.Failure<Language>("language should be exactly two characters long");
        return Result.Success(new Language(language));

    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
