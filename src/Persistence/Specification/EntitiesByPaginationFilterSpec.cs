using Ardalis.Specification;
using Domain.Extensions.Models;

namespace Persistence.Specification;

public class EntitiesByPaginationFilterSpec<T, TResult> : EntitiesByBaseFilterSpec<T, TResult>
{
    public EntitiesByPaginationFilterSpec(PaginationFilter filter)
        : base(filter) =>
        Query.PaginateBy(filter);
}

public class EntitiesByPaginationFilterSpec<T> : EntitiesByBaseFilterSpec<T>
{
    public EntitiesByPaginationFilterSpec(PaginationFilter filter)
        : base(filter) =>
        Query.PaginateBy(filter);
}
public class EntitiesByPaginationSpec<T> : Specification<T>
{
    public EntitiesByPaginationSpec(PaginationFilter filter) =>
        Query.PaginateBy(filter);
}
public class EntitiesByPaginationSpec<T,TResult> : Specification<T, TResult>
{
    public EntitiesByPaginationSpec(PaginationFilter filter) =>
        Query.PaginateBy(filter);
}