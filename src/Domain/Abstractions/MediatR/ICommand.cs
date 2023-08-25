using LanguageExt.Common;
using MediatR;

namespace Domain.Abstractions.MediatR;

public interface ICommand:IRequest<Result<bool>>
{
}
public interface ICommand<TResult>:IRequest<Result<TResult>>
{
}