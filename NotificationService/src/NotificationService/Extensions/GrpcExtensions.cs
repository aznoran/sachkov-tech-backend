using Grpc.Core;
using NotificationService.Grpc;
using NotificationService.HelperClasses;
using static NotificationService.HelperClasses.Error;

namespace NotificationService.Extensions;

public static class GrpcExtensions
{
    public static RpcException ToGrpcResponse(this Error error)
    {
        var statusCode = GetStatusCodeForErrorTypeGrpc(error.Type);
        return new RpcException(new Status(statusCode, error.Message));
    }
    private static StatusCode GetStatusCodeForErrorTypeGrpc(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => StatusCode.InvalidArgument,
            ErrorType.NotFound => StatusCode.NotFound,
            ErrorType.Conflict => StatusCode.FailedPrecondition,
            ErrorType.Failure => StatusCode.Internal,
            _ => StatusCode.Internal
        };

    public static bool IsValidGuid(this GuidGrpc guid, out RpcException exception)
    {
        exception = null!;

        if (Guid.TryParse(guid.Guid, out _))
            return true;

        var status = new Status
            (
                StatusCode.InvalidArgument,
                $"Guid is not valid: {guid.Guid}"
            );
        exception = new RpcException(status);

        return false;
    }

    public static bool IsValidGuid(this IEnumerable<GuidGrpc> guidCollection, out RpcException exception, string collectionName = "")
    {
        exception = null!;

        var problematicGuid = guidCollection
            .FirstOrDefault(x => x.IsValidGuid(out _) == false);

        if (problematicGuid == null)
            return true;

        var status = new Status
            (
                StatusCode.InvalidArgument,
                $"Guid in {collectionName} collection is not valid: {problematicGuid}"
            );
        exception = new RpcException(status);

        return false;
    }
}
