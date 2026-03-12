using System.Net.Mime;
using DocumentComposition.Application.Data;
using DocumentComposition.Domain.Binder;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Application.Binders;

public class AddDocumentCommandHandler(IUnitOfWork unitOfWork, IBinderRepository repository)
{
    public async Task<Result> HandleAsync(AddDocumentCommand command)
    {
        var documentId = DocumentId.Create();

        return await repository.GetByIdAsync(command.Id)
            .BindAsync(binder => binder.AddDocument(documentId, command.SourceUri, new ContentType(MediaTypeNames.Image.Png)))
            .EnsureAsync(unitOfWork.CommitWorkAsync);
    }
}
