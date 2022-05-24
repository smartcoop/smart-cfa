namespace Smart.FA.Catalog.Infrastructure.Helpers;

public interface IMinIoLinkGenerator
{
    /// <summary>
    /// Create the relative path to the profile picture of the trainer out of his id
    /// </summary>
    /// <param name="trainerId">The id of the trainer</param>
    /// <returns>The relative path to the profile picture of the trainer</returns>
    string GenerateTrainerProfilePictureUrl(int trainerId, string imageFormat);

    /// <summary>
    /// Fetch the absolute path to the profile picture image of the trainer
    /// </summary>
    /// <param name="relativeTrainerProfilePictureUrl"> The relative path of the trainer's profile picture</param>
    /// <returns>The absolute path to the trainer's profile picture</returns>
    string GetAbsoluteTrainerProfilePictureUrl(string? relativeTrainerProfilePictureUrl);

    /// <summary>
    /// Get default training profile picture relative path
    /// </summary>
    /// <returns>The relative path to a default picture profile</returns>
    string GetDefaultRelativeProfilePictureImageUrl();

    /// <summary>
    /// Create the relative path of the user chart out of his id
    /// </summary>
    /// <param name="userChartRevisionId">The id of the user chart revision</param>
    /// <returns>The relative path to the user chart revision</returns>
    string GenerateUserChartRevisionUrl(int userChartRevisionId);

    /// <summary>
    /// Fetch the absolute path of the user chart revision
    /// </summary>
    /// <param name="userChartId"> The relative path of the trainer's profile picture</param>
    /// <returns>The absolute path to the trainer's profile picture</returns>
    string GetAbsoluteUserChartUrl(int userChartId);
}
