using LanguageExt.Common;
using MediatR;

namespace Domain.Abstractions.MediatR;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand,Result<bool>>
    where TCommand : ICommand{}
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand:ICommand<TResponse>
{
}