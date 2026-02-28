using DocumentComposition.Application.Data;
using DocumentComposition.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace DocumentComposition.Infrastructure.Data;

internal class EfCoreUnitOfWork<TDbContext>(TDbContext dbContext) : IUnitOfWork where TDbContext : DbContext
{
    public virtual async Task<Result> CommitWorkAsync()
    {
        try
        {
            await dbContext.SaveChangesAsync();

            return Result.Successful();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Failed(new ConcurrencyError("A concurrency failure has occurred."));
        }
    }
}
