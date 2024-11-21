using Amazon.S3;
using Amazon.S3.Model;
using FileService.Contracts;
using FileService.Endpoints;
using FileService.MongoDataAccess;

namespace FileService.Features;

public static class GetFilesPresignedUrls
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("files/presigned-urls", Handler);
        }
    }

    private static async Task<IResult> Handler(
        GetFilesPresignedUrlsRequest request,
        IAmazonS3 s3Client,
        IFilesRepository filesRepository,
        CancellationToken cancellationToken)
    {
        var files = await filesRepository.Get(request.FileIds, cancellationToken);

        List<FileResponse> fileResponses = [];
        foreach (var file in files)
        {
            var presignedRequest = new GetPreSignedUrlRequest
            {
                BucketName = "bucket",
                Key = file.StoragePath,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddDays(2),
                Protocol = Protocol.HTTP
            };

            var presignedUrl = await s3Client.GetPreSignedURLAsync(presignedRequest);

            fileResponses.Add(new FileResponse(file.Id, presignedUrl));
        }

        return Results.Ok(fileResponses);
    }
}