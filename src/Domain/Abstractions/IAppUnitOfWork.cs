using Domain.Entities.Base;

namespace Domain.Abstractions;

public interface IAppUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}