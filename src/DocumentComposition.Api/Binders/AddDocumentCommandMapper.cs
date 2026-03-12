using DocumentComposition.Application.Binders;
using DocumentComposition.Domain.Binder;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Api.Binders;

public class AddDocumentCommandMapper
{
    public Result<AddDocumentCommand> Map(Guid id, AddDocumentRequest request)
    {
        return BinderId.From(id)
            .Combine(StorageUri.From(request.SourceUri))
            .Map(result =>
            {
                var (id, sourceUri) = result;

                return new AddDocumentCommand
                {
                    Id = id,
                    SourceUri = sourceUri
                };
            });
    }
}
