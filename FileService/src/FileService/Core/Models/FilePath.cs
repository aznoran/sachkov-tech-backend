namespace FileService.Core.Models;

public class FilePath
{
    public string FullPath { get; }

    public string BucketName => GetBucketName();

    public string Prefix => GetPrefix();

    public string FileName => GetFileName();

    public string FileNameWithPrefix => Prefix + "/" + FileName;

    public string FileExtension => GetFileExtension();

    public FilePath(string fullPath)
    {
        var pathParts = fullPath.Split('/')
            .Where(p => string.IsNullOrWhiteSpace(p) == false);

        if (pathParts.Count() < 2)
            throw new ArgumentException("The file path must consist of at least 2 elements.");

        FullPath = string.Join("/", pathParts);
    }

    public FilePath(string bucketName, string fileName, string? prefix = null)
    {
        FullPath = string.Join("/", bucketName, prefix, fileName);
    }

    public static implicit operator string(FilePath filePath) =>
        filePath.FullPath;

    public static implicit operator FilePath(string filePath) =>
        new FilePath(filePath);

    private string GetBucketName()
    {
        var filePathParts = FullPath.Split('/');

        return filePathParts.First();
    }

    private string GetPrefix()
    {
        var filePathParts = FullPath.Split('/');

        var prefixParts = filePathParts.Take(new Range(1, filePathParts.Length - 1));

        return string.Join("/", prefixParts);
    }

    private string GetFileName()
    {
        var filePathParts = FullPath.Split('/');

        return filePathParts.Last();
    }

    private string GetFileExtension()
    {
        return Path.GetExtension(FullPath);
    }
}

