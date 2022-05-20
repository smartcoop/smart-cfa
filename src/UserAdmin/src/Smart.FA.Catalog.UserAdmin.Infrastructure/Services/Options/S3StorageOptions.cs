namespace Smart.FA.Catalog.UserAdmin.Infrastructure.Services.Options;

public class S3StorageOptions
{
    public AWSCredentials Credentials { get; set; } = null!;

    public AWSOptions AWS { get; set; } = null!;

    public string DefaultTrainerProfilePictureName { get; set; } = null!;

    public long FileSizeLimit { get; set; }

    private string _imageBucketName = null!;

    public string ImageBucketName
    {
        get => _imageBucketName;
        set => _imageBucketName = value.ToLower();
    }
}

public class AWSCredentials
{
    public string AccessKey { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
}

public class AWSOptions
{
    public string RegionEndpoint { get; set; } = null!;
    public string ServiceUrl { get; set; } = null!;
    public bool ForcePathStyle { get; set; }
}
