using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Domain.Abstractions.Repositories;
using Domain.Entities.Base;
using MongoDB.Driver;

namespace Persistence.Repositories.Base;

public abstract class BaseMongoRepository<T> : IRepository<T> where T : Entity
{
    private readonly MongoClient _client;
    private readonly MongoClientSettings _settings;
    private readonly IMongoCollection<T> _collection;

    public BaseMongoRepository(string connectionUri, string databaseName, string collectionName)
    {
        _settings = MongoClientSettings.FromConnectionString(connectionUri);
        _settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        _client = new MongoClient(_settings);
        _collection = _client.GetDatabase(databaseName).GetCollection<T>( collectionName ??typeof(T).Name.ToLower());
    }

    public async Task<T> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = new CancellationToken())
        where TId : notnull
    {
        var filter = Builders<T>.Filter
            .Eq("_id", id);
        var result= await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return result;
    }

    public async Task<T> GetBySpecAsync(ISpecification<T> specification,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task<TResult> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task<T> FirstOrDefaultAsync(ISpecification<T> specification,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task<TResult> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task<T> SingleOrDefaultAsync(ISingleResultSpecification<T> specification,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task<TResult> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<T, TResult> specification,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task<List<T>> ListAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task<List<T>> ListAsync(ISpecification<T> specification,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task<int> CountAsync(ISpecification<T> specification,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task<bool> AnyAsync(ISpecification<T> specification,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task<bool> AnyAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = new CancellationToken())
    {
        await _collection.InsertOneAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        throw new System.NotImplementedException();
    }

    public async Task AddRangAsync(T[] entities, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public async Task AddRangAsync(List<T> entities, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }
}