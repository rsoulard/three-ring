using DocumentComposition.Shared.Results;

namespace DocumentComposition.Domain.Binder;

public interface IBinderRepository
{
    /// <summary>
    /// Retrieve a binder by its Id.
    /// </summary>
    /// <param name="id">The Id of the binder to retrieve.</param>
    /// <returns>A result that will contain the Binder on success or an Error on failure.</returns>
    Task<Result<Binder>> GetByIdAsync(BinderId id);

    /// <summary>
    /// Add a Binder to the repository.
    /// </summary>
    /// <param name="binder">The Binder to add to the repository.</param>
    /// <returns>A result that will contain the Binder on success or an Error on failure.</returns>
    Task<Result<Binder>> AddAsync(Binder binder);
}
