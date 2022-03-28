using Smart.FA.Catalog.Core.Domain;

namespace Smart.FA.Catalog.Application.Extensions;

public static class ProfilePictureExtensions
{
    public static string GenerateTrainerProfilePictureName(this Trainer trainer) =>
        $"{trainer.Id}/{trainer.Name.LastName}-{trainer.Name.FirstName}";

    public static string GenerateDefaultTrainerProfilePictureName(this Trainer trainer) =>
        "default_image";
}
