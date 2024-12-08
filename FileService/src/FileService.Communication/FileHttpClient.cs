using CSharpFunctionalExtensions;
using FileService.Contracts;
using System.Net;
using System.Net.Http.Json;

namespace FileService.Communication;

public class FileHttpClient(HttpClient httpClient) : IFileService
{
    /// <summary>
    /// Function get URLs to upload files from repository.
    /// </summary>
    /// <param name="request">Contains list of Guid files identificators needs to upload</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Function returns list of record FileResponse with FileId and URL</returns>
    public async Task<Result<IReadOnlyList<FileResponse>, string>> GetFilesPresignedUrls(
        GetFilesPresignedUrlsRequest request, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync("files/presigned-urls", request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return "Fail to get files presigned urls";
        }

        var fileResponse = await response.Content.ReadFromJsonAsync<IEnumerable<FileResponse>>(cancellationToken);

        return fileResponse?.ToList() ?? [];
    }

    /// <summary>
    /// Function start multipart upload large file 
    /// </summary>
    /// <param name="request">Contains file name, type (e.g. *.jpeg or *.mov), size in bytes</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Function returns record FileResponse with file upload session identifire and URL to upload file</returns>
    public async Task<Result<FileResponse, string>> StartMultipartUpload(
       StartMultipartUploadRequest request, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync("files/multipart", request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return "Fail to start multipart upload";
        }

        var fileResponse = await response.Content.ReadFromJsonAsync<FileResponse>(cancellationToken);

        return fileResponse!;
    }

    /// <summary>
    /// Function complete multipart upload large file
    /// </summary>
    /// <param name="request">Contains file upload session identifire and list of pairs: file part number and it's URL</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Function returns record FileResponse with FileId and file URL in repository</returns>
    public async Task<Result<FileResponse, string>> CompleteMultipartUpload(
       CompleteMultipartRequest request, CancellationToken cancellationToken)
    {
        var response = await httpClient.PostAsJsonAsync(
            $"files/{request.UploadId}/complete-multipart",
            request,
            cancellationToken);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return "Fail to complete multipart upload";
        }

        var fileResponse = await response.Content.ReadFromJsonAsync<FileResponse>(cancellationToken);

        return fileResponse!;
    }
}
