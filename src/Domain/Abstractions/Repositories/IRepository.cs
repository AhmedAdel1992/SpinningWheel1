using Ardalis.Specification;
using Domain.Entities.Base;

namespace Domain.Abstractions.Repositories;

public interface IRepository<T> : IRepositoryBase<T> where T : class,IEntity
{
    Task AddRangAsync(T[] entities, CancellationToken cancellationToken = default);
    Task AddRangAsync(List<T> entities, CancellationToken cancellationToken = default);
    // Task<bool> ExistAsync<TId>(TId id, CancellationToken cancellationToken = default);
}