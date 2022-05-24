using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Infrastructure.Services.Options;

namespace Smart.FA.Catalog.Infrastructure.Services;

public class S3StorageService : IS3StorageService
{
    private readonly S3StorageOptions _options;
    private readonly IAmazonS3 _client;

    public S3StorageService(IAmazonS3 client, IOptions<S3StorageOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    /// <summary>
    /// Create a bucket on the S3 server
    /// </summary>
    private async Task CreateBucketAsync(CancellationToken cancellationToken)
    {
        if (await _client.DoesS3BucketExistAsync(_options.ImageBucketName)) return;
        PutBucketRequest request = new() {BucketName = _options.ImageBucketName};
        await _client.PutBucketAsync(request, cancellationToken);
    }

    /// <summary>
    /// Delete a file from a bucket
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="fileName">Name of the file to upload</param>
    public async Task DeleteAsync(string fileName, CancellationToken cancellationToken)
    {
        DeleteObjectRequest request = new() {BucketName = _options.ImageBucketName, Key = fileName};
        await _client.DeleteObjectAsync(request, cancellationToken);
    }

    /// <summary>
    /// Upload a file to a bucket with a specified name
    /// </summary>
    /// <param name="fileStream">Stream of the file to upload</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="fileName">Name of the file to upload</param>
    public async Task UploadAsync(Stream fileStream, string fileName, CancellationToken cancellationToken)
    {
        await CreateBucketAsync(cancellationToken);
        TransferUtilityUploadRequest request = new() {Key = fileName, InputStream = fileStream, BucketName = _options.ImageBucketName};
        TransferUtility fileTransferUtility = new(_client);
        await fileTransferUtility.UploadAsync(request, cancellationToken);
    }

    /// <summary>
    /// Get the stream of a file with a specific name in the S3 storage
    /// </summary>
    /// <param name="fileName">File to fetch</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The Stream of the file</returns>
    public async Task<Stream?> GetAsync(string fileName, CancellationToken cancellationToken)
    {
        GetObjectRequest request = new() {BucketName = _options.ImageBucketName, Key = fileName};
        StreamResponse? response = await _client.GetObjectAsync(request, cancellationToken);
        return response?.ResponseStream;
    }

    /// <summary>
    /// Get the pre-signed url
    /// </summary>
    /// <param name="fileName">File to fetch</param>
    /// <param name="expirationDate">Date of expiration of the generated token</param>
    /// <returns>The Stream of the file</returns>
    public Uri? GetPreSignedUrl(string fileName, DateTime expirationDate)
    {
        var absoluteUrl =_client.GetPreSignedURL(new GetPreSignedUrlRequest {Key = fileName, BucketName = _options.ImageBucketName, Expires = expirationDate}) ?? null;
        return  absoluteUrl is null ? null : new Uri(absoluteUrl);
    }
}
