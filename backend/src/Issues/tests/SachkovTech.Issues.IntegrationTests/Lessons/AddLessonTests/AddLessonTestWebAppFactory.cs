using CSharpFunctionalExtensions;
using FileService.Communication;
using FileService.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NSubstitute;

namespace SachkovTech.Issues.IntegrationTests.Lessons.AddLessonTests;

public class AddLessonTestWebAppFactory : IntegrationTestsWebAppFactory
{
    private readonly IFileService _fileServiceMock = Substitute.For<IFileService>();

    protected override void ConfigureDefaultServices(IServiceCollection services)
    {
        base.ConfigureDefaultServices(services);

        var fileServiceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IFileService));
        if (fileServiceDescriptor != null)
            services.Remove(fileServiceDescriptor);

        services.AddTransient<IFileService>(_ => _fileServiceMock);
    }

    public void SetupSuccessFileServiceMock()
    {
        var response = new FileResponse(Guid.NewGuid(), "testUrl");
        _fileServiceMock
            .CompleteMultipartUpload(Arg.Any<CompleteMultipartRequest>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success<FileResponse, string>(response));
    }

    public void SetupFailureFileServiceMock()
    {
        _fileServiceMock
            .CompleteMultipartUpload(Arg.Any<CompleteMultipartRequest>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure<FileResponse, string>("Failed to upload file"));
    }
}