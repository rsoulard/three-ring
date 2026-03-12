using System.ComponentModel.DataAnnotations;

namespace DocumentComposition.Api.Binders;

public class AddDocumentRequest
{
    [Required]
    public Uri SourceUri { get; init; } = null!;
}
