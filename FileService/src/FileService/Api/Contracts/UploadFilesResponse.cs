namespace FileService.Api.Contracts;

public record UploadFilesResponse(IEnumerable<Guid> UploadedFileIds, int UploadFilesCount, int NotUploadedFilesCount);