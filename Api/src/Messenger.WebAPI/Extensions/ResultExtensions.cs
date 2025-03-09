using Messenger.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Extensions
{
    public static class ResultExtensions
    {
        public static ProblemDetails ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException();
            }

            var error = result.Error;

            var problemDetails = new ProblemDetails
            {
                Type = GetType(error.ErrorType),
                Title = GetTitle(error.ErrorType),
                Detail = error.Description,
                Status = GetStatusCode(error.ErrorType)
            };

            if (error.ErrorType != ErrorType.InternalFailure)
            {
                problemDetails.Extensions = new Dictionary<string, object?>
                {
                    { "errors", new { code = error.Code, description = error.Description } }
                };

                if (result is IValidationResult validationResult)
                {
                    problemDetails.Extensions["errors"] = validationResult.Errors
                        .Select(e => new { e.Code, e.Description })
                        .ToArray();
                }
            }

            return problemDetails;
        }

        public static IActionResult ToActionResult(this Result result)
        {
            var problemDetails = result.ToProblemDetails();

            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };
        }

        private static int GetStatusCode(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.OperationFailure => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status403Forbidden,

                _ => StatusCodes.Status500InternalServerError
            };
        }

        private static string GetTitle(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => "Bad Request",
                ErrorType.NotFound => "Not Found",
                ErrorType.OperationFailure => "Bad Request",
                ErrorType.Unauthorized => "Unauthorized",

                _ => "Internal Server Error"
            };
        }

        private static string GetType(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                ErrorType.OperationFailure => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                ErrorType.Unauthorized => "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",

                _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };
        }
    }
}
