namespace Smart.FA.Catalog.Core.Services;

public interface IS3StorageService
{
    public Task UploadAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
    public Task<Stream?> GetAsync(string fileName, CancellationToken cancellationToken = default);
    public Task DeleteAsync(string fileName, CancellationToken cancellationToken = default);
}
