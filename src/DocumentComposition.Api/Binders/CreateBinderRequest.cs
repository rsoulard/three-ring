using System.ComponentModel.DataAnnotations;

namespace DocumentComposition.Api.Binders;

public class CreateBinderRequest
{
    [Required]
    public string Name { get; init; } = null!;
}
