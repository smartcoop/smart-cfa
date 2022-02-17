using Core.Exceptions;

namespace Web.Validators;

public static class Errors
{
    public static class TrainingViewModel
    {
        public static Error EmptyMethodology() =>
            new("training.empty.methodology", "A training needs info about its methodology");
        public static Error EmptyGoal() =>
            new("training.empty.goal", "A training needs info about its goal");
        public static Error EmptyTitle() =>
            new("training.empty.title", "A training needs a title");
        public static Error MissingTargetAudience() =>
            new("training.missing.audience", "At least one target audience is required in a training.");
        public static Error MissingSlotType() =>
            new("training.missing.slot.type", "At least one slot type is required in a training.");
        public static Error MissingType() =>
            new("training.missing.type", "At least one type is required in a training.");
    }
}
