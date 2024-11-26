using System.Net.Http.Json;
using FileService.Contracts;
using FluentAssertions;

namespace FileService.IntegrationTests;

public class GetFilesPresignedUrlsTest : FileServiceTestsBase
{
    public GetFilesPresignedUrlsTest(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_Files_Presigned_Urls_Test()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;
        
        var fileId = await UploadFile(cancellationToken);

        var request = new GetFilesPresignedUrlsRequest(new List<Guid> { fileId });

        // act
        var response = await AppHttpClient.PostAsJsonAsync("files/presigned-urls", request, cancellationToken);

        // assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var filesResponse = await response.Content.ReadFromJsonAsync<List<FileResponse>>(cancellationToken);
        filesResponse!.Count.Should().Be(1);
        filesResponse[0].FileId.Should().Be(fileId);
    }
}