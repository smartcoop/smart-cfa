namespace Smart.FA.Catalog.Core.Exceptions;

public static partial class Errors
{
    public static class Training
    {
        public static Error NotFound(int id) => new("training.not.found", $"Training with id {id} was not found");
        public static Error TooManyAssignments() =>
            new("trainer.too.many.assignments", "Training cannot have more than 2 assignments");

        public static Error AlreadyAssigned(string trainingName) =>
            new("trainer.already.assigned.to.training", $"Trainer is already assigned to the training '{trainingName}'");

        public static Error InvalidState(string name) => new("training.invalid.state", $"Invalid state: '{name}'");

        public static Error TrainerIsInvalid() => new("trainer.in.training.is.invalid", "Trainer is invalid");

        public static Error ValidationError(string message) => new("training.validation.error", message);

        public static Error DuplicateLanguageDetails() => new("duplicate.language.details.in.training", "A description for that language already exists");

        public static Error EmptyTitle() => new("empty.title.in.training", "Title cannot be empty, null or consists only of whitespaces");

        public static Error NoTargetAudience() => new("null.target.audience.in.training", "Target audiences should not be null");

        public static Error NoVatExemption() => new("null.vat.exemption.in.training", "VAT exemption should not be null");

        public static Error NoTopics() => new("empty.topic.in.training", "Topics should not be null");

        public static Error NoTrainer() => new("empty.trainer.in.training", "At least one trainer should be assigned to the trainer");





    }
    public static class Language
    {
        public static Error InvalidLength() => new("language.invalid.length", $"language should be exactly two characters long");
    }
}
