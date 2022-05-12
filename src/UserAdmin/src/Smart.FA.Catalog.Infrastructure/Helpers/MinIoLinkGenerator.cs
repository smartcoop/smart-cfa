using HashidsNet;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Infrastructure.Services.Options;

namespace Smart.FA.Catalog.Infrastructure.Helpers;

public class MinIoLinkGenerator : IMinIoLinkGenerator
{
    private readonly S3StorageOptions _storageOptions;

    public MinIoLinkGenerator(IOptions<S3StorageOptions> storageOptions)
    {
        _storageOptions = storageOptions.Value;
    }

    /// <inheritdoc />
    public string CreateTrainerProfilePictureUrl(int trainerId)
    {
        // Generate unique salt value to avoid decoding
        var hashIds = new Hashids(MinioLinkDefaultSaltValues.TrainerProfilePicture, 8);
        return Path.Combine("trainers/", "profile-image/", $"{hashIds.Encode(trainerId)}.jpg");
    }

    /// <inheritdoc />
    public string GetFullTrainerProfilePictureUrl(string relativeTrainerProfilePictureUrl) =>
        Path.Combine($"{_storageOptions.AWS.ServiceUrl}/", $"{_storageOptions.ImageBucketName}/", relativeTrainerProfilePictureUrl);

    /// <inheritdoc />
    public string GetDefaultFullProfilePictureImageUrl()
    {
        // Generate unique salt value to avoid decoding
        var hashIds = new Hashids(MinioLinkDefaultSaltValues.TrainerProfilePicture, 8);
        return Path.Combine("trainers/", "profile-image/",$"{hashIds.Encode(0)}.jpg");
    }

    /// <inheritdoc />
    public string CreateUserChartRevisionUrl(int userChartRevisionId)
    {
        // No salt because we should be able to decode a string
        var hashIds = new Hashids(MinioLinkDefaultSaltValues.UserChartRevision, 8);
        return Path.Combine("usercharts/", $"{hashIds.Encode(userChartRevisionId)}.pdf");
    }

    /// <inheritdoc />
    public string GetFullUserChartUrl(int userChartId) => Path.Combine($"{_storageOptions.AWS.ServiceUrl}/", $"{_storageOptions.ImageBucketName}/", CreateUserChartRevisionUrl(userChartId));
}

public static class MinioLinkDefaultSaltValues
{
    public const string UserChartRevision = "User chart revision salt";
    public const string TrainerProfilePicture = "Trainer profile picture salt";
}
