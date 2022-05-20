namespace Smart.FA.Catalog.UserAdmin.Domain.Services;

/// <summary>
/// Executes operations that need to be performed upon booting.
/// </summary>
public interface IBootStrapService
{
    /// <summary>
    /// Applies migrations and then seeds the database.
    /// If an error occurs while the migration happens, the seeding won't be executed.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="Exception">A generic exception in case the operation failed.</exception>
    Task ApplyMigrationsAndSeedAsync();

    /// <summary>
    /// Seed-upload a default image in the S3 storage.
    /// The default image will be served for members who have yet to upload a personal profile picture.
    /// </summary>
    Task AddDefaultTrainerProfilePictureImage(string webRootPath);

    /// <summary>
    /// Seed-upload a default user chart in the S3 storage.
    /// The default user chart needs to be approved at first startup
    /// </summary>
    /// <remarks>
    /// This method should be triggered after the database seed since it relies on an actual record of the database to find the related name of the user chart
    /// </remarks>
    Task AddDefaultUserChart(string webRootPath);
}
