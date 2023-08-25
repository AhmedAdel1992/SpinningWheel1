using LanguageExt.Common;
using MediatR;

namespace Domain.Abstractions.MediatR;

public interface IQuery<TResult>:IRequest<Result<TResult>>
{
}