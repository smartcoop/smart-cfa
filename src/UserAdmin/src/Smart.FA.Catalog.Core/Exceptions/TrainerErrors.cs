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

        public static Error BiographyIsTooLong(int id,int biographyLength, int maxLength) => new("trainer.biography.too.long", $"Biography is too long for trainer {id} ({biographyLength} character long with a max length of {maxLength})");

        public static Error TitleIsTooLong(int id, int titleLength, int maxLength) => new("trainer.title.too.long", $"Title is too long for trainer {id} ({titleLength} character long with a max length of {maxLength})");


        public static class ProfileImage
        {
            public static Error CantUpload(string fileName) => new("cant.upload.profile.image", $"Profile image {fileName} can't be uploaded");
            public static Error CantDelete(string fileName) => new("cant.delete.profile.image", $"Profile image {fileName} can't be deleted");
            public static Error CantGet() => new("cant.fetch.profile.image", "Profile image can't be fetched");
            public static Error UrlTooLong(int id, int urlLength, int maxLength) => new("profile.image.url.too.long", $"Title is too long for trainer {id} ({urlLength} character long with a max length of {maxLength})");
        }
    }
}
