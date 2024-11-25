using Amazon.S3.Model;
using FluentAssertions;

namespace FileService.IntegrationTests;

public class GetPresignedUrlTest : TestsBase
{
    public GetPresignedUrlTest(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_Presigned_Url()
    {
        // arrange
        CancellationToken cancellationToken = new CancellationTokenSource().Token;
        
        var key = await UploadFileToMinio(cancellationToken);
        
        // act
        var response = await HttpClient.GetAsync($"files/{key}/presigned", cancellationToken);
        
        // assert
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    private async Task<string> UploadFileToMinio(CancellationToken cancellationToken = default)
    {
        var putBucketRequest = new PutBucketRequest
        {
            BucketName = "bucket"
        };

        await S3Client.PutBucketAsync(putBucketRequest, cancellationToken);

        var filePath = "..\\..\\..\\test.mp4";
        var key = "test.mp4";

        var uploadFileRequest = new PutObjectRequest
        {
            BucketName = "bucket",
            Key = key,
            FilePath = filePath
        };

        await S3Client.PutObjectAsync(uploadFileRequest, cancellationToken);

        return key;
    }
}