using Amazon.S3;
using Amazon.S3.Model;
using AutoFixture;
using FileService.Core;
using FileService.MongoDataAccess;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.IntegrationTests;

public class FileServiceTestsBase : IClassFixture<IntegrationTestsWebFactory>
{
    protected readonly IAmazonS3 S3Client;
    protected readonly IFilesRepository Repository;
    protected readonly FileMongoDbContext MongoDbContext;
    protected readonly HttpClient AppHttpClient;
    protected readonly IServiceScope Scope;
    protected readonly HttpClient HttpClient;

    protected FileServiceTestsBase(
        IntegrationTestsWebFactory factory)
    {
        Scope = factory.Services.CreateScope();
        Repository = Scope.ServiceProvider.GetRequiredService<IFilesRepository>();
        S3Client = Scope.ServiceProvider.GetRequiredService<IAmazonS3>();
        MongoDbContext = Scope.ServiceProvider.GetRequiredService<FileMongoDbContext>();
        AppHttpClient = factory.CreateClient();
        HttpClient = new HttpClient();
    }
    
    protected async Task<Guid> UploadFile(CancellationToken cancellationToken = default)
    {
        var file = new FileInfo("..\\..\\..\\test.mp4");
        
        var fileData = new FileData
        {
            Id = Guid.NewGuid(),
            StoragePath = "test.mp4",
            FileSize = file.Length,
            ContentType = "video/mp4",
            UploadDate = DateTime.Now
        };

        await Repository.Add(fileData, cancellationToken);

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

        return fileData.Id;
    }
}