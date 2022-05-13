namespace Smart.FA.Catalog.Showcase.Web.Options;

public class MinIOOptions
{
    public const string SectionName = "MinIO";

    public string BaseUrl { get; set; } = null!;

    public string BucketName { get; set; } = null!;

    public string GenerateMinIoTrainerProfileUrl(string? trainerDetailsProfileImagePath)
    {
        return !string.IsNullOrEmpty(trainerDetailsProfileImagePath)
            ? $"{BaseUrl}/{BucketName}/{trainerDetailsProfileImagePath.TrimStart('/')}"
            : Constants.DefaultTrainerProfilePicturePath;
    }
}
