using DocumentComposition.Application.Data;
using DocumentComposition.Domain.Binder;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Application.Binders;

public class CreateBinderCommandHandler(IUnitOfWork unitOfWork, IBinderRepository repository)
{
    public async Task<Result<BinderId>> HandleAsync(CreateBinderCommand command)
    {
        var binderId = BinderId.Create();

        return await Binder.Create(binderId, command.Name)
            .BindAsync(repository.AddAsync)
            .EnsureAsync(unitOfWork.CommitWorkAsync)
            .MapAsync(binder => binder.Id);
    }
}
