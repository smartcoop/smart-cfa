namespace Core.Exceptions;

public static partial class Errors
{
    public static class Training
    {
        public static Error NotFound(int id) => new("training.not.found", $"Training with id {id} was not found");
        public static Error TooManyAssignments() =>
            new("trainer.too.many.assignments", "Training cannot have more than 2 assignments");

        public static Error AlreadyAssigned(string trainingName) =>
            new("trainer.already.assigned", $"Trainer is already assigned to the training '{trainingName}'");

        public static Error InvalidState(string name) => new("invalid.state", $"Invalid state: '{name}'");

        public static Error TrainerIsInvalid() => new("trainer.is.invalid", "Trainer is invalid");

        public static Error ValidationError(string message) => new("validation.error", message);
    }
    public static class Language
    {
        public static Error InvalidLength() => new("language.invalid.length", $"language should be exactly two characters long");
    }
}
