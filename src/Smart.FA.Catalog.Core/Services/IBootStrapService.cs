namespace Smart.FA.Catalog.Core.Services;

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
}
