using DocumentComposition.Application.Binders;
using DocumentComposition.Domain.Binder;
using DocumentComposition.Shared.Results;

namespace DocumentComposition.Api.Binders;

public class BinderIdQueryMapper
{
    public Result<BinderIdQuery> Map(Guid id)
    {
        return BinderId.From(id)
            .Map(binderId => new BinderIdQuery
            {
                Id = binderId
            });
    }
}
