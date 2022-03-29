namespace Smart.FA.Catalog.Infrastructure.Services.Options;

public class S3StorageOptions
{
    public AWSCredentials Credentials { get; set; }

    public AWSOptions AWS { get; set; }

    public string DefaultTrainerProfilePictureName { get; set; }

    public long FileSizeLimit { get; set; }

    private string _imageBucketName;

    public string ImageBucketName
    {
        get => _imageBucketName;
        set => _imageBucketName = value.ToLower();
    }
}

public class AWSCredentials
{
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
}

public class AWSOptions
{
    public string RegionEndpoint { get; set; }
    public string ServiceUrl { get; set; }
    public bool ForcePathStyle { get; set; }
}
