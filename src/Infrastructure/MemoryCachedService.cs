using Common.DependencyInjection.Interfaces;
using Common.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure;

public class MemoryCachedService : ICachedService, IScoped
{
    private readonly IMemoryCache _cache;

    public MemoryCachedService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void Dispose()=> _cache.Dispose();

    public ICacheEntry CreateEntry(object key)=> _cache.CreateEntry(key);

    public void Remove(object key)=> _cache.Remove(key);


    public bool TryGetValue(object key, out object value)=> _cache.TryGetValue(key, out value);

    public Task<TItem> GetOrCreateAsync<TItem>(object key, Func<ICacheEntry, Task<TItem>> factory)=> _cache.GetOrCreateAsync(key, factory);
    public TItem GetOrCreate<TItem>(object key, Func<ICacheEntry, TItem> factory)=> _cache.GetOrCreate(key, factory);
}