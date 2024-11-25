using System.Net.Http.Json;
using Amazon.S3.Model;
using FileService.Contracts;
using FileService.Core;
using FluentAssertions;

namespace FileService.IntegrationTests;

public class GetFilesPresignedUrlsTest : TestsBase
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
        var response = await HttpClient.PostAsJsonAsync("files/presigned-urls", request, cancellationToken);

        // assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var filesResponse = await response.Content.ReadFromJsonAsync<List<FileResponse>>(cancellationToken);
        filesResponse!.Count.Should().Be(1);
        filesResponse[0].FileId.Should().Be(fileId);
    }

    private async Task<Guid> UploadFile(CancellationToken cancellationToken = default)
    {
        var file = new FileInfo("..\\..\\..\\test.mp4");
        
        var fileId = await AddFileDataToDatabase(file, cancellationToken);

        await UploadFileToMinio(file, cancellationToken);

        return fileId;
    }
    
    private async Task<Guid> AddFileDataToDatabase(FileInfo file, CancellationToken cancellationToken = default)
    {
        var fileData = new FileData
        {
            Id = Guid.NewGuid(),
            StoragePath = "test.mp4",
            FileSize = file.Length,
            ContentType = "video/mp4",
            UploadDate = DateTime.Now
        };

        await Repository.Add(fileData, cancellationToken);

        return fileData.Id;
    }

    private async Task UploadFileToMinio(FileInfo file, CancellationToken cancellationToken = default)
    {
        var putBucketRequest = new PutBucketRequest
        {
            BucketName = "bucket"
        };

        await S3Client.PutBucketAsync(putBucketRequest, cancellationToken);

        var uploadFileRequest = new PutObjectRequest
        {
            BucketName = "bucket",
            Key = file.Name,
            FilePath = file.FullName
        };

        await S3Client.PutObjectAsync(uploadFileRequest, cancellationToken);
    }
}