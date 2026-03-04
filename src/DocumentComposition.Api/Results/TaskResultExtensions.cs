using Microsoft.AspNetCore.Mvc;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Api.Results;

public static class TaskResultExtensions
{
    public static async Task<IActionResult> ToActionResult(this Task<Result> task)
    {
        var result = await task;
        return result.ToActionResult();
    }

    public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> task)
    {
        var result = await task;
        return result.ToActionResult();
    }
}
