namespace FileService.Data.Dtos;

public record UploadFilesResult(string BucketName, string FileName, FilePath FilePath, long FileSize);

