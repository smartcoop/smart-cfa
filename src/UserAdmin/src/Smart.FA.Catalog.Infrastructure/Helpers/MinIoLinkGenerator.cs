using HashidsNet;
using Microsoft.Extensions.Options;
using Smart.FA.Catalog.Infrastructure.Services.Options;

namespace Smart.FA.Catalog.Infrastructure.Helpers;

public class MinIoLinkGenerator : IMinIoLinkGenerator
{
    private const string TrainerRelativeFolder = "trainers/profile-image/";
    private const string UserChartRelativeFolder = "usercharts/";

    private readonly S3StorageOptions _storageOptions;

    public MinIoLinkGenerator(IOptions<S3StorageOptions> storageOptions)
    {
        _storageOptions = storageOptions.Value;
    }

    /// <inheritdoc />
    public string GenerateTrainerProfilePictureUrl(int trainerId, string imageFormat)
    {
        // Generate unique salt value to avoid decoding
        var hashIds = new Hashids(Guid.NewGuid().ToString(), 8);
        return Path.Combine(TrainerRelativeFolder, $"{hashIds.Encode(trainerId)}{imageFormat}");
    }

    /// <inheritdoc />
    public string GetAbsoluteTrainerProfilePictureUrl(string? relativeTrainerProfilePictureUrl)
    {
        return Path.Combine($"{_storageOptions.AWS.ServiceUrl}/", $"{_storageOptions.ImageBucketName}/", relativeTrainerProfilePictureUrl ?? GetDefaultRelativeProfilePictureImageUrl());
    }

    /// <inheritdoc />
    public string GetDefaultRelativeProfilePictureImageUrl()
    {
        // No unique salt because we should be able to decode a string
        var hashIds = new Hashids(MinioLinkDefaultSaltValues.TrainerProfilePicture, 8);
        return Path.Combine(TrainerRelativeFolder, $"{hashIds.Encode(0)}.png");
    }

    /// <inheritdoc />
    public string GenerateUserChartRevisionUrl(int userChartRevisionId)
    {
        // No unique salt because we should be able to decode a string
        var hashIds = new Hashids(MinioLinkDefaultSaltValues.UserChartRevision, 8);
        return Path.Combine(UserChartRelativeFolder, $"{hashIds.Encode(userChartRevisionId)}.pdf");
    }

    /// <inheritdoc />
    public string GetAbsoluteUserChartUrl(int userChartId) => Path.Combine($"{_storageOptions.AWS.ServiceUrl}/", $"{_storageOptions.ImageBucketName}/", GenerateUserChartRevisionUrl(userChartId));
}

public static class MinioLinkDefaultSaltValues
{
    public const string UserChartRevision = "User chart revision salt";
    public const string TrainerProfilePicture = "Trainer profile picture salt";
}
