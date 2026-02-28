using DocumentComposition.Domain;
using DocumentComposition.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace DocumentComposition.Infrastructure.Data;

internal abstract class EfCoreBaseAggregateRepository<TEntity, TId>(DbContext dbContext) where TEntity : AggregateRoot<TId> where TId : notnull
{
    protected DbSet<TEntity> DbSet { get; private set; } = dbContext.Set<TEntity>();

    public async Task<Result<TEntity>> GetByIdAsync(TId id)
    {
        var entity = await DbSet.FindAsync(id);

        if (entity is null)
        {
            return Result.Failed<TEntity>(new NotFoundError($"{nameof(TEntity)} not found"));
        }

        return Result.From(entity);
    }

    public Result Delete(TEntity entity)
    {
        try
        {
            DbSet.Remove(entity);
            return Result.Successful();
        }
        catch (Exception)
        {
            return Result.Failed<TEntity>(new InfrastructureError("Unable to delete the entity."));
        }
    }

    public async Task<Result<TEntity>> AddAsync(TEntity entity)
    {
        try
        {
            await DbSet.AddAsync(entity);
            return Result.From(entity);
        }
        catch (Exception)
        {
            return Result.Failed<TEntity>(new InfrastructureError("Unable to add the entity."));
        }
    }
}
