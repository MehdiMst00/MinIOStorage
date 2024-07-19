namespace MinIOStorage.MinioConfiguration;

public class MinioOptions
{
    public const string Section = "Storage";

    public string Endpoint { get; set; } = default!;

    public string DefaultBucketName { get; set; } = default!;

    public string AccessKey { get; set; } = default!;

    public string SecretKey { get; set; } = default!;
}
