using DocumentComposition.Application.Binders;
using DocumentComposition.Domain.Binder;
using DocumentComposition.Infrastructure.Data;
using DocumentComposition.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace DocumentComposition.Infrastructure.Binders;

internal class BinderReadRepository(DocumentCompositionDbContext dbContext) : IBinderReadRepository
{
    public async Task<Result<BinderDto>> GetByIdAsync(BinderId id)
    {
        var query = from binder in dbContext.Set<Binder>()
                    where binder.Id == id
                    select new BinderDto
                    {
                        Id = binder.Id.Value,
                        Name = binder.Name,
                        Status = binder.Status.ToString(),
                        OutputUri = binder.OutputUri == null ? null : binder.OutputUri.Value
                    };

        var queryResult = await query.AsNoTracking()
            .FirstOrDefaultAsync();

        if (queryResult is null)
        {
            return Result.Failed<BinderDto>(new NotFoundError("Binder not found."));
        }

        return Result.From(queryResult);
    }
}
