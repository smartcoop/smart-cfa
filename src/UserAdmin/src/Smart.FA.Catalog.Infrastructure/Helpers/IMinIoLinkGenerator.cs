namespace Smart.FA.Catalog.Infrastructure.Helpers;

public interface IMinIoLinkGenerator
{
    /// <summary>
    /// Create the relative URL of the profile picture of the trainer out of his id
    /// </summary>
    /// <param name="trainerId">The id of the trainer</param>
    /// <returns>The relative url to the profile picture of the trainer</returns>
    string CreateTrainerProfilePictureUrl(int trainerId);

    /// <summary>
    /// Fetch the absolute URL of the profile picture image of the trainer
    /// </summary>
    /// <param name="relativeTrainerProfilePictureUrl"> The relative path of the trainer's profile picture</param>
    /// <returns>The absolute URL of the trainer's profile picture</returns>
    string GetFullTrainerProfilePictureUrl(string relativeTrainerProfilePictureUrl);

    /// <summary>
    /// Get default training profile picture URL
    /// </summary>
    /// <returns>The URL for a default picture profile</returns>
    string GetDefaultFullProfilePictureImageUrl();

    /// <summary>
    /// Create the relative URL of the user chart out of his id
    /// </summary>
    /// <param name="userChartRevisionId">The id of the user chart revision</param>
    /// <returns>The relative url of the user chart revision</returns>
    string CreateUserChartRevisionUrl(int userChartRevisionId);

    /// <summary>
    /// Fetch the absolute URL of the user chart revision
    /// </summary>
    /// <param name="userChartId"> The relative path of the trainer's profile picture</param>
    /// <returns>The absolute URL of the trainer's profile picture</returns>
    string GetFullUserChartUrl(int userChartId);
}
