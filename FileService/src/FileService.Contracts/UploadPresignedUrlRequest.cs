namespace FileService.Contracts;

public record UploadPresignedUrlRequest(string FileName, string ContentType, long Size);