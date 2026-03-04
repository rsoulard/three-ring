using Microsoft.AspNetCore.Mvc;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Api.Results;

public static class ErrorExtensions
{
    public static ProblemDetails ToProblemDetails(this Error error, int status, string title)
    {
        return new ProblemDetails
        {
            Status = status,
            Title = title,
            Type = error.GetType().Name,
            Detail = error.Message
        };
    }
}
