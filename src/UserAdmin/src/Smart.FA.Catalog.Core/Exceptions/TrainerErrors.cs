namespace Smart.FA.Catalog.Core.Exceptions;

public static partial class Errors
{
    public static class Trainer
    {
        public static Error TrainerDoesntExist(int id) => new("no.trainer.with.id.exists", $"No trainer with id {id} exists ");

        public static Error TrainerIsTransient() => new("trainer.is.transient", "The trainer cannot be transient");

        public static Error TrainerAlreadyAssignedToTraining(int trainerId, int trainingId) =>
            new("trainer.already.assigned", $"The trainer {trainerId} was already assigned to training {trainingId}");

        public static Error TrainerNeverAssignedToTraining(int trainerId, int trainingId) => new("trainer.never.assigned", $"The trainer {trainerId} was never assigned to training {trainingId}");

        public static Error BiographyIsTooLong(int id) => new("trainer.biography.too.long", $"Biography is too long for trainer {id}");

        public static Error TitleIsTooLong(int id) => new("trainer.title.too.long", $"Title is too long for trainer {id}");


        public static class ProfileImage
        {
            public static Error CantUpload(string fileName) => new("cant.upload.profile.image", $"Profile image {fileName} can't be uploaded");
            public static Error CantDelete(string fileName) => new("cant.delete.profile.image", $"Profile image {fileName} can't be deleted");
            public static Error CantGet() => new("cant.fetch.profile.image", "Profile image can't be fetched");
            public static Error UrlTooLong(int id) => new("profile.image.url.too.long", $"Title is too long for trainer {id}");
        }
    }
}
