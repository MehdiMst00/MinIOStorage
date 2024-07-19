using Microsoft.Extensions.Options;
using Minio.DataModel.Args;
using Minio.Exceptions;
using MinIOStorage.MinioConfiguration;

namespace MinIOStorage.Services;

public class StorageService(
    ILogger<StorageService> logger,
    IMinioClient minio,
    IOptions<MinioOptions> options) : IStorageService
{
    private readonly MinioOptions _options = options.Value;

    /// <inheritdoc/>
    public Task<UploadObjectModel?> UploadAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        return UploadAsync(file, _options.DefaultBucketName, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<UploadObjectModel?> UploadAsync(IFormFile file, string bucket, CancellationToken cancellationToken = default)
    {
        try
        {
            // Make a bucket on the server, if not already present.
            var beArgs = new BucketExistsArgs().WithBucket(bucket);
            bool isBucketExist = await minio.BucketExistsAsync(beArgs, cancellationToken).ConfigureAwait(false);
            if (!isBucketExist)
            {
                var mbArgs = new MakeBucketArgs()
                    .WithBucket(bucket);

                await minio.MakeBucketAsync(mbArgs, cancellationToken).ConfigureAwait(false);
            }

            using var fileStream = file.OpenReadStream();

            // Set object name
            var objectName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // Upload a file to bucket.
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(file.ContentType);

            var objResponse = await minio.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[Bucket] Successfully uploaded {ObjectName}", objResponse.ObjectName);

            return new UploadObjectModel { ObjectName = objResponse.ObjectName };
        }
        catch (MinioException e)
        {
            logger.LogError("[Bucket] File Upload Error: {Message}", e.Message);
            return null;
        }
    }

    /// <inheritdoc/>
    public Task<DownloadObjectModel?> DownloadAsync(
        string objectName,
        CancellationToken cancellationToken = default)
    {
        return DownloadAsync(objectName, _options.DefaultBucketName, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<DownloadObjectModel?> DownloadAsync(
        string objectName,
        string bucket,
        CancellationToken cancellationToken = default)
    {
        MemoryStream? mt = null;

        try
        {
            mt = new MemoryStream();

            var args = new GetObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName)
                .WithCallbackStream(async (stream) =>
                {
                    await stream.CopyToAsync(mt);
                    await stream.DisposeAsync();
                });

            var objStat = await minio.GetObjectAsync(args, cancellationToken).ConfigureAwait(false);
            logger.LogInformation("Downloaded the object {objectName} in bucket {bucketName}", objectName, bucket);

            return new DownloadObjectModel { Stream = mt, ContentType = objStat.ContentType };
        }
        catch (Exception e)
        {
            logger.LogError("[Bucket] Exception While Download: {ex}", e);
            return null;
        }
        finally
        {
            mt?.Seek(0, SeekOrigin.Begin);
        }
    }
}