using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories.Base;

public abstract class BaseRepository<T> : RepositoryBase<T>, IRepository<T> where T :  class,IEntity
{
    private readonly DbContext _dbContext;

    public BaseRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    protected IDbConnection Connection => _dbContext.Database.GetDbConnection();
    public override async Task<T> AddAsync(T entity, CancellationToken cancellationToken =default)
    {
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
        return entity;
    }

    public override Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Update(entity);
        return Task.CompletedTask;
    }

    public async Task AddRangAsync(T[] entities, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
    }

    public async Task AddRangAsync(List<T> entities, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
    }
}


