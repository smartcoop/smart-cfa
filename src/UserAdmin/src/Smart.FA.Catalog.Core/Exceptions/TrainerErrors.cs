namespace Smart.FA.Catalog.Core.Exceptions;

public static partial class Errors
{
    public static class Trainer
    {
        public static Error TrainerDoesntExist(int id) => new("no.trainer.with.id.exists", $"No trainer with id {id} exists ");

        public static class ProfileImage
        {
            public static Error CantUpload(string fileName) => new("cant.upload.profile.image", $"Profile image {fileName} can't be uploaded");
            public static Error CantDelete(string fileName) => new("cant.delete.profile.image", $"Profile image {fileName} can't be deleted");
            public static Error CantGet(string fileName) => new("cant.fetch.profile.image", $"Profile image can't be fetched");
        }
    }
}
