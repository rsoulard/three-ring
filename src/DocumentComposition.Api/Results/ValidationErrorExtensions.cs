using Microsoft.AspNetCore.Mvc;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Api.Results;

public static class ValidationErrorExtensions
{
    public static ValidationProblemDetails ToValidationProblemDetails(this ValidationError error)
    {
        var errorDictionary = new Dictionary<string, string[]>(){
            {error.PropertyName, [error.Message]}
        };

        return new(errorDictionary);
    }
}
