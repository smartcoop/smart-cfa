namespace Core.Services;

/// <summary>
/// Execute operations that need to be done upon booting.
/// </summary>
public interface IBootStrapService
{
    /// <summary>
    /// Applies migrations and seed the database.
    /// </summary>
    /// <returns>A taskk representing the asynchronous operation.</returns>
    Task SeedAndApplyMigrationsAsync();
}
