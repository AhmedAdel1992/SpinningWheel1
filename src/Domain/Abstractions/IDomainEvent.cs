using MediatR;

namespace Domain.Abstractions;

public interface IDomainEvent:INotification
{
}
// public interface IDomainEvent<TId>:IDomainEvent
// {
//     public TId Id { get; set; }
// }