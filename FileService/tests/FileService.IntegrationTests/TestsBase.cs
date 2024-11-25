using Amazon.S3;
using AutoFixture;
using FileService.MongoDataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.IntegrationTests;

public class TestsBase : IClassFixture<IntegrationTestsWebFactory>
{
    protected readonly IAmazonS3 S3Client;
    protected readonly IFilesRepository Repository;
    protected readonly FileMongoDbContext MongoDbContext;
    protected readonly Fixture Fixture;
    protected readonly HttpClient HttpClient;
    protected readonly IServiceScope Scope;

    protected TestsBase(IntegrationTestsWebFactory factory)
    {
        Scope = factory.Services.CreateScope();
        Repository = Scope.ServiceProvider.GetRequiredService<IFilesRepository>();
        S3Client = Scope.ServiceProvider.GetRequiredService<IAmazonS3>();
        MongoDbContext = Scope.ServiceProvider.GetRequiredService<FileMongoDbContext>();
        Fixture = new Fixture();
        HttpClient = factory.CreateClient();
    }
}