using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Api.Results;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccessful)
        {
            return new OkResult();
        }

        return TranslateError(result);
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccessful)
        {
            return new OkObjectResult(result.Value);
        }

        return TranslateError(result);
    }
    
    private static IActionResult TranslateError(Result result)
    {
        return result.Error switch
        {
            NotFoundError error => new NotFoundObjectResult(error.ToProblemDetails(StatusCodes.Status404NotFound, "Not Found")),
            ConcurrencyError error => new ConflictObjectResult(error.ToProblemDetails(StatusCodes.Status409Conflict, "Concurrency Conflict")),
            ValidationError error => new BadRequestObjectResult(error.ToValidationProblemDetails()),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}
