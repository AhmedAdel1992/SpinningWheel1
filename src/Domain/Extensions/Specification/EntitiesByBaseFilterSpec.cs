using Domain.Extensions.Models;
using Ardalis.Specification;
using Mapster;

namespace Domain.Extensions.Specification;

public class EntitiesByBaseFilterSpec<T, TResult> : Specification<T, TResult>
{
    public EntitiesByBaseFilterSpec(BaseFilter filter)
    {
        Query.SearchBy(filter);
        Query.Select(s => s.Adapt<TResult>());
    }
}

public class EntitiesByBaseFilterSpec<T> : Specification<T>
{
    public EntitiesByBaseFilterSpec(BaseFilter filter) =>
        Query.SearchBy(filter);
}