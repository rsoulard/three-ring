using DocumentComposition.Domain.Binder;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Application.Binders;

public interface IBinderReadRepository
{
    Task<Result<BinderDto>> GetByIdAsync(BinderId binderId);
}
