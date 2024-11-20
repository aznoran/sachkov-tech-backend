using CSharpFunctionalExtensions;
using FileService.Contracts;

namespace FileService.Communication;

public interface IFileService
{
    Task<Result<IReadOnlyList<FileResponse>, string>> GetFilesPresignedUrls(
        GetFilesPresignedUrlsRequest request, CancellationToken cancellationToken = default);

    Task<Result<FileResponse, string>> StartMultipartUpload(
       StartMultipartUploadRequest request, CancellationToken cancellationToken = default);

    Task<Result<FileResponse, string>> CompleteMultipartUpload(
       CompleteMultipartRequest request, CancellationToken cancellationToken = default);
}
