namespace FileService.Contracts;

public record GetFilesPresignedUrlsRequest(IEnumerable<Guid> FileIds);