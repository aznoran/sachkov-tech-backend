using System.Net.Http.Json;
using AutoFixture;
using FileService.Contracts;
using FluentAssertions;

namespace FileService.IntegrationTests;

public class UploadPresignedUrlTest : TestsBase
{
    public UploadPresignedUrlTest(IntegrationTestsWebFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task Upload_Presigned_Url()
    {
        // arrange
        var request = Fixture.Create<UploadPresignedUrlRequest>();
        
        var cancellationToken = new CancellationTokenSource().Token;

        // act
        var response = await HttpClient.PostAsJsonAsync("files/presigned", request, cancellationToken);

        // assert
        response.IsSuccessStatusCode.Should().BeTrue();
    }
}