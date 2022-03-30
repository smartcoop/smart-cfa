using CSharpFunctionalExtensions;

namespace Smart.FA.Catalog.Core.Exceptions
{
    public sealed class Error : ValueObject
    {
        private const string Separator = "||";

        public string Code { get; }
        public string Message { get; }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }

        public string Serialize()
        {
            return $"{Code}{Separator}{Message}";
        }

        public static Error Deserialize(string serialized)
        {
            if (serialized == "A non-empty request body is required.")
                return Errors.General.ValueIsRequired();

            var data = serialized.Split(new[] {Separator}, StringSplitOptions.RemoveEmptyEntries);

            if (data.Length < 2)
                throw new Exception($"Invalid error serialization: '{serialized}'");

            return new Error(data[0], data[1]);
        }
    }

    public static partial class Errors
    {
        public static class General
        {
            public static Error NotFound(long? id = null)
            {
                var forId = id == null ? "" : $" for Id '{id}'";
                return new Error("record.not.found", $"Record not found{forId}");
            }

            public static Error MissingField(string fieldName)
            {
                return new Error("missing.field", $"field {fieldName} is missing");
            }

            public static Error ValueIsInvalid() => new("value.is.invalid", "Value is invalid");

            public static Error ValueIsRequired() => new("value.is.required", "Value is required");

            public static Error InvalidLength(string? name = null)
            {
                var label = name == null ? " " : " " + name + " ";
                return new Error("invalid.string.length", $"Invalid{label}length");
            }

            public static Error CollectionIsTooSmall(int min, int current)
            {
                return new Error(
                    "collection.is.too.small",
                    $"The collection must contain {min} items or more. It contains {current} items.");
            }

            public static Error CollectionIsTooLarge(int max, int current)
            {
                return new Error(
                    "collection.is.too.large",
                    $"The collection must contain {max} items or more. It contains {current} items.");
            }

            public static Error UnexpectedHandlerError()
            {
                return new Error(
                    "unexpected.handler.error",
                    "An unexpected error occurred during the executing of the request");
            }

            public static Error InternalServerError(string message)
            {
                return new Error("internal.server.error", message);
            }

            public static Error GuardClauseBlockage(string message) => new("guard.clause.blockage", message);
        }

        public static class User
        {
            public static Error NotFound(string? id = null)
            {
                var forId = id == null ? "" : $" for Id '{id}'";
                return new Error("user.not.found", $"User not found{forId}");
            }
        }
    }
}
