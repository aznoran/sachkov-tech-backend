using Microsoft.AspNetCore.Mvc;
using NotificationService.HelperClasses;
using static NotificationService.HelperClasses.Error;

namespace NotificationService.Extensions
{
    public static class ResponseExtensions
    {
        public static ActionResult ToResponse(this Error error)
        {
            var statusCode = GetStatusCodeForErrorType(error.Type);

            var envelope = Envelope.Error([error]);

            return new ObjectResult(envelope)
            {
                StatusCode = statusCode
            };
        }
        private static int GetStatusCodeForErrorType(ErrorType errorType) =>
            errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
