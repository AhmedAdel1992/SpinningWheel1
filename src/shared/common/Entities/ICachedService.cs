using Microsoft.Extensions.Caching.Memory;

namespace Common.Entities;

public interface ICachedService
{
    ICacheEntry CreateEntry(object key);
    void Remove(object key);
    bool TryGetValue(object key, out object value);
    Task<TItem> GetOrCreateAsync<TItem>(object key, Func<ICacheEntry, Task<TItem>> factory);
    TItem GetOrCreate<TItem>(object key, Func<ICacheEntry, TItem> factory);
}