namespace MinIOStorage.Models;

public record DownloadObjectModel
{
    public Stream Stream { get; set; } = null!;
    public string ContentType { get; set; } = null!;
}
