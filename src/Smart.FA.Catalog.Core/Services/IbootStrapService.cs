namespace Core.Services;

/// <summary>
/// Executes operations that need to be done upon booting.
/// </summary>
public interface IBootStrapService
{
    /// <summary>
    /// Applies migrations and then seed the database.
    /// If an error occurs while the migration happens, the seeding won't be executed.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ApplyMigrationsAndSeedAsync();
}
