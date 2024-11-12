using Amazon.S3;
using Amazon.S3.Model;
using FileService.Core;
using FileService.Endpoints;
using FileService.Jobs;
using FileService.MongoDataAccess;
using Hangfire;

namespace FileService.Features;

public static class TestHangfire
{
    public sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("test", Handler);
        }
    }

    private static IResult Handler(
        CancellationToken cancellationToken)
    {
        var jobId = BackgroundJob.Schedule<ConfirmConsistencyJob>(j => j.Execute(Guid.NewGuid(), "key"), TimeSpan.FromSeconds(5));

        return Results.Ok(jobId);
    }
}