using System.Net;
using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using FileService.Contracts;

namespace FileService.Communication;

public class FileHttpClient(HttpClient httpClient)
{
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
}