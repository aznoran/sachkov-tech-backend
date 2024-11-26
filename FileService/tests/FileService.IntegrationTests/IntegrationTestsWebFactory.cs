using Amazon.S3;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Testcontainers.Minio;
using Testcontainers.MongoDb;

namespace FileService.IntegrationTests;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MongoDbContainer _dbContainer = new MongoDbBuilder()
        .WithImage("mongo")
        .WithUsername("mongoadmin")
        .WithPassword("mongopassword").Build();

    private readonly MinioContainer _minioContainer = new MinioBuilder()
        .WithImage("minio/minio")
        .WithUsername("minioadmin")
        .WithPassword("minioadmin")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var minio = services.SingleOrDefault(s =>
                s.ServiceType == typeof(IAmazonS3));

            var mongoClient = services.SingleOrDefault(s =>
                s.ServiceType == typeof(IMongoClient));

            if (minio is not null)
                services.Remove(minio);

            if (mongoClient is not null)
                services.Remove(mongoClient);
            
            var port = _minioContainer.GetMappedPublicPort(9000);

            services.AddSingleton<IAmazonS3>(_ =>
            {
                var config = new AmazonS3Config
                {
                    ServiceURL = $"http://{_minioContainer.Hostname}:{port}",
                    ForcePathStyle = true,
                    UseHttp = true
                };

                return new AmazonS3Client("minioadmin", "minioadmin", config);
            });

            services.AddSingleton<IMongoClient>(new MongoClient(_dbContainer.GetConnectionString()));
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _minioContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
        
        await _minioContainer.StopAsync();
        await _minioContainer.DisposeAsync();
    }
}