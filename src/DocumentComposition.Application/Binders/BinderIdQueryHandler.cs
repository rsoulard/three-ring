using DocumentComposition.Shared.Results;

namespace DocumentComposition.Application.Binders;

public class BinderIdQueryHandler(IBinderReadRepository binderReadRepository)
{
    public async Task<Result<BinderDto>> HandleAsync(BinderIdQuery query)
    {
        return await binderReadRepository.GetByIdAsync(query.Id);
    }
}
