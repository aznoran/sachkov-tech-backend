using System.Net.Http.Json;
using Amazon.S3.Model;
using FileService.Contracts;
using FluentAssertions;
using MongoDB.Driver;
using Xunit.Abstractions;

namespace FileService.IntegrationTests;

public class MultipartUploadFileTest : TestsBase
{
    private record StartMultipartUploadResult(string Key, string UploadId);

    private record UploadPresignedPartUrlResult(string Key, string Url);

    private record CompleteMultipartUploadResult(string Id, string Location);
    
    public MultipartUploadFileTest(IntegrationTestsWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Multipart_Upload_File()
    {
        // arrange
        FileInfo fileInfo = new FileInfo("..\\..\\..\\test.mp4");
        
        var cancellationToken = new CancellationTokenSource().Token;
        
        await CreateBucket(cancellationToken);
        
        var startMultipartResult = await StartMultipartUpload(fileInfo, cancellationToken);
        
        int chunkSize = 1024 * 1024 * 5;
        
        var parts = new List<PartETagInfo>();
        int partNumber = 1;
        
        await using var stream = fileInfo.OpenRead();

        for (int start = 0; start < fileInfo.Length; start += chunkSize)
        {
            byte[] chunk = new byte[chunkSize];
            await stream.ReadAsync(chunk, 0, chunkSize, cancellationToken);

            var uploadPresignedPartUrlResult = await UploadPresignedPartUrl(
                startMultipartResult,
                partNumber,
                cancellationToken);

            var eTag = await UploadFilePartToMinio(
                uploadPresignedPartUrlResult,
                chunk,
                cancellationToken); 
            
            parts.Add(new PartETagInfo(partNumber, eTag!));
            partNumber++;
        }
        
        // act
        var completeMultipartResult = await CompleteMultipartUpload(
            startMultipartResult,
            parts,
            cancellationToken);

        // assert
        var file = MongoDbContext.Files.Find(f => f.StoragePath == completeMultipartResult.Id).FirstOrDefault();

        file.Should().NotBeNull();
    }

    private async Task CreateBucket(CancellationToken cancellationToken = default)
    {
        var request = new PutBucketRequest
        {
            BucketName = "bucket"
        };

        await S3Client.PutBucketAsync(request, cancellationToken);
    }

    private async Task<StartMultipartUploadResult> StartMultipartUpload(
        FileInfo fileInfo, 
        CancellationToken cancellationToken = default)
    {
        var request = new StartMultipartUploadRequest(
            fileInfo.Name,
            "video/mp4",
            fileInfo.Length);
        
        var response = await HttpClient
            .PostAsJsonAsync("files/multipart", request, cancellationToken);

        response.EnsureSuccessStatusCode();
        
        return (await response.Content
            .ReadFromJsonAsync<StartMultipartUploadResult>(cancellationToken))!;
    }
    
    private async Task<UploadPresignedPartUrlResult> UploadPresignedPartUrl(
        StartMultipartUploadResult startMultipartResult,
        int partNumber,
        CancellationToken cancellationToken = default)
    {
        var request = new UploadPresignedPartUrlRequest
            (startMultipartResult!.UploadId, partNumber);

        var response = await HttpClient
            .PostAsJsonAsync(
                $"files/{startMultipartResult.Key}/presigned-part",
                request,
                cancellationToken);

        response.EnsureSuccessStatusCode();

        return (await response.Content
            .ReadFromJsonAsync<UploadPresignedPartUrlResult>(cancellationToken))!;
    }

    private async Task<string> UploadFilePartToMinio(
        UploadPresignedPartUrlResult uploadPresignedPartUrlResult,
        byte[] chunk,
        CancellationToken cancellationToken = default)
    {
        var httpClient = new HttpClient();   
        
        using var content = new ByteArrayContent(chunk);
                
        var response = await httpClient
            .PutAsync(uploadPresignedPartUrlResult!.Url, content, cancellationToken);

        response.EnsureSuccessStatusCode();
                
        return response.Headers.GetValues("etag").FirstOrDefault()!;
    }

    private async Task<CompleteMultipartUploadResult> CompleteMultipartUpload(
        StartMultipartUploadResult startMultipartResult,
        List<PartETagInfo> parts,
        CancellationToken cancellationToken = default)
    {
        var completeMultipartRequest = new CompleteMultipartRequest(startMultipartResult!.UploadId, parts);

        var response =  await HttpClient.PostAsJsonAsync(
            $"files/{startMultipartResult.Key}/complete-multipart",
            completeMultipartRequest,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return (await response.Content
            .ReadFromJsonAsync<CompleteMultipartUploadResult>(cancellationToken))!;
    }
}