using FileService.Core.Models;

namespace FileService.Core.Dtos;

public record UploadFilesResult(string BucketName, string FileName, FilePath FilePath, long FileSize);

