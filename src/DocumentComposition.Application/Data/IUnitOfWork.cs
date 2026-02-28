using DocumentComposition.Shared.Results;

namespace DocumentComposition.Application.Data;

public interface IUnitOfWork
{
    /// <summary>
    /// Save the current changes and commit them.
    /// </summary>
    /// <remarks>
    /// Intended to be called only once when the work is complete.
    /// </remarks>
    Task<Result> CommitWorkAsync();
}
