namespace MinIOStorage.Services;

public interface IStorageService
{
    /// <summary>
    /// Upload a file to default bucket
    /// </summary>
    /// <param name="file"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UploadObjectModel?> UploadAsync(IFormFile file,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Upload a file to spec bucket
    /// </summary>
    /// <param name="file"></param>
    /// <param name="bucket"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UploadObjectModel?> UploadAsync(IFormFile file,
        string bucket,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get file from default bucket
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DownloadObjectModel?> DownloadAsync(string objectName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get object from spec bucket
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="bucket"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DownloadObjectModel?> DownloadAsync(string objectName,
        string bucket,
        CancellationToken cancellationToken = default);
}