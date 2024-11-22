using CSharpFunctionalExtensions;
using FileService.Communication;
using FileService.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace SachkovTech.Issues.IntegrationTests.Lessons.AddLessonTests;

public class AddLessonTestWebAppFactory : IntegrationTestsWebAppFactory
{
    private Mock<IFileService> _fileServiceMock = default!;

    protected override void ConfigureDefaultServices(IServiceCollection services)
    {
        base.ConfigureDefaultServices(services);

        var fileServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IFileService));
        if (fileServiceDescriptor != null)
            services.Remove(fileServiceDescriptor);

        _fileServiceMock = new Mock<IFileService>();
        services.AddTransient<IFileService>(_ => _fileServiceMock.Object);
    }

    public void SetupSuccessFileServiceMock()
    {
        var response = new FileResponse(Guid.NewGuid(), "testUrl");
        _fileServiceMock
            .Setup(f => f.CompleteMultipartUpload(It.IsAny<CompleteMultipartRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<FileResponse, string>(response));
    }

    public void SetupFailureFileServiceMock()
    {
        _fileServiceMock
            .Setup(f => f.CompleteMultipartUpload(It.IsAny<CompleteMultipartRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<FileResponse, string>("Failed to upload file"));
    }
}