namespace MinIOStorage.Controllers;

[ApiController]
public class StorageController(IStorageService storageService) : ControllerBase
{
    /// <summary>
    /// Upload a file to default bucket
    /// </summary>
    /// <param name="file"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UploadObjectModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken = default)
    {
        var uploadModel = await storageService.UploadAsync(file, cancellationToken);

        if (uploadModel == null)
            return BadRequest("File upload error");

        return Ok(uploadModel);
    }

    /// <summary>
    /// Upload a file to spec bucket
    /// </summary>
    /// <param name="file"></param>
    /// <param name="bucket"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("{bucket}/upload")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UploadObjectModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(IFormFile file, string bucket, CancellationToken cancellationToken = default)
    {
        var uploadModel = await storageService.UploadAsync(file, bucket, cancellationToken);

        if (uploadModel == null)
            return BadRequest("File upload error");

        return Ok(uploadModel);
    }

    /// <summary>
    /// Get file from default bucket
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="inline"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("download/{objectName}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(string objectName, bool inline = false, CancellationToken cancellationToken = default)
    {
        var downloadModel = await storageService.DownloadAsync(objectName, cancellationToken);

        if (downloadModel == null)
            return NotFound("File not found");

        var cd = new System.Net.Mime.ContentDisposition
        {
            FileName = objectName,
            Inline = inline,
        };
        Response.Headers.Append("Content-Disposition", cd.ToString());
        return File(downloadModel.Stream, downloadModel.ContentType);
    }

    /// <summary>
    /// Get object from spec bucket
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="bucket"></param>
    /// <param name="inline"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{bucket}/download/{objectName}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(string objectName, string bucket, bool inline = false, CancellationToken cancellationToken = default)
    {
        var downloadModel = await storageService.DownloadAsync(objectName, bucket, cancellationToken);

        if (downloadModel == null)
            return NotFound("File not found");

        var cd = new System.Net.Mime.ContentDisposition
        {
            FileName = objectName,
            Inline = inline,
        };
        Response.Headers.Append("Content-Disposition", cd.ToString());
        return File(downloadModel.Stream, downloadModel.ContentType);
    }
}