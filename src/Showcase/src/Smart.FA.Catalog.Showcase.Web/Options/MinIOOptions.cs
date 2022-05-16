namespace Smart.FA.Catalog.Showcase.Web.Options;

public class MinIOOptions
{
    public const string SectionName = "MinIO";

    public string BaseUrl { get; set; } = null!;

    public string BucketName { get; set; } = null!;

    //TODO having a bit of logic is not the best to have, even tho it is acceptable. Moving the MinIO link generator would be better tho.
    public string GenerateMinIoTrainerProfileUrl(string? trainerDetailsProfileImagePath)
    {
        return !string.IsNullOrEmpty(trainerDetailsProfileImagePath)
            ? $"{BaseUrl}/{BucketName}/{trainerDetailsProfileImagePath.TrimStart('/')}"
            : Constants.DefaultTrainerProfilePicturePath;
    }
}
