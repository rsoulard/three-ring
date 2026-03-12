using DocumentComposition.Domain.Binder;

namespace DocumentComposition.Application.Binders;

public class AddDocumentCommand
{
    public BinderId Id { get; init; } = null!;
    public StorageUri SourceUri { get; init; } = null!;
}
