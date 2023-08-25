using LanguageExt.Common;
using MediatR;

namespace Domain.Abstractions.MediatR;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery:IQuery<TResponse>
{
}