// using System;
// using Common.Logging;
// using Domain.Contracts;
// using Domain.Entities;
// using Flynas.Contracts.Refund;
// using MassTransit;
//
// namespace Persistence.StateMachines;
//
// public class RefundStateMachine : MassTransitStateMachine<RefundState>
// {
//     private readonly ILoggerAdapter<RefundStateMachine> _logger;
//     public State Submitted { get; set; }
//     public State Accepted { get; set; }
//     public State Rejected { get; set; }
//     public State ProcessBookingRefund { get; set; }
//     public State BookingRefunded { get; set; }
//     public State ProcessPaymentRefund { get; set; }
//     public State PaymentRefunded { get; set; }
//
//     public State Payed { get; set; }
//     public State Completed { get; set; }
//     public State Failed { get; set; }
//
//     public Event<RefundState> RefundState { get; set; }
//
//     public Event<RefundSubmitted> RequestSubmitted { get; set; }
//     public Event<RefundCompletedByFinance> CompletedByFinance { get; set; }
//     public Event<ChangedFlightContract> ChangedFlight { get; set; }
//     public Event<AddedFeeContract> AddedFee { get; set; }
//     public Event<AddedPaymentContract> AddedPayment { get; set; }
//     // public Event<BookingCommitContract> BookingCommit { get; set; }
//     // public Event<PaymentRefundedContract> PaymentRefundCommit { get; set; }
//
//     public Event<CheckRefundState> CheckRefundState { get; set; }
//
//     public RefundStateMachine(ILoggerAdapter<RefundStateMachine> logger)
//     {
//         _logger = logger;
//
//         Event(() => CompletedByFinance, x =>
//             x.CorrelateById(r => r.Message.Id));
//
//         InstanceState(x => x.CurrentState);
//         SubmittedHandler();
//         ChangeFlightHandler();
//         AddedFeeHandler();
//         AddedPaymentHandler();
//         RefundStateHandler();
//         BookingCommittedHandler();
//         
//         // DuringAny(When(PaymentRefundCommit).Finalize());
//         SetCompletedWhenFinalized();
//     }
//
//     private void RefundStateHandler()
//     {
//         Event(() => CheckRefundState, x =>
//             {
//                 x.CorrelateById(ctx => ctx.Message.RequestId)
//                     .OnMissingInstance(i =>
//                         i.ExecuteAsync(async e =>
//                             await e.RespondAsync<RefundNotFound>(
//                                 new { Message = "Refund not found" }
//                             )
//                         )
//                     );
//             }
//         );
//
//         DuringAny(When(CheckRefundState)
//             .RespondAsync(r => r.Init<RefundStatus>(new
//                 {
//                     RequestId = r.Saga.CorrelationId,
//                     State = r.Saga.CurrentState,
//                 }
//             )));
//     }
//     private void ChangeFlightHandler()
//     {
//         Event(() => ChangedFlight, x =>
//             x.CorrelateById(r => r.Message.RequestId));
//
//         During(Submitted, Ignore(ChangedFlight));
//         During(Rejected, Ignore(ChangedFlight));
//
//         DuringAny(When(ChangedFlight)
//             .Then(ctx =>
//             {
//                 _logger.LogInformation("Flight changed {RequestId}", ctx.Message.RequestId);
//                 ctx.Saga.Flights.Add(ctx.Message);
//             })
//             .TransitionTo(ProcessBookingRefund)
//         );
//     }
//     private void AddedPaymentHandler()
//     {
//         Event(() => AddedPayment, x =>
//             x.CorrelateById(r => r.Message.RequestId));
//
//         During(Submitted, Ignore(AddedPayment));
//         During(Rejected, Ignore(AddedPayment));
//         During(ProcessBookingRefund, Ignore(AddedPayment));
//
//         DuringAny(When(AddedPayment)
//             .Then(ctx =>
//             {
//                 _logger.LogInformation("Payment added {RequestId}", ctx.Message.RequestId);
//                 ctx.Saga.Payments.Add(ctx.Message);
//             })
//             .TransitionTo(ProcessBookingRefund)
//         );
//     }
//     private void AddedFeeHandler()
//     {
//         Event(() => AddedFee, x =>
//             x.CorrelateById(r => r.Message.RequestId));
//
//         During(Submitted, Ignore(AddedFee));
//         During(Rejected, Ignore(AddedFee));
//
//         DuringAny(When(AddedFee)
//             .Then(ctx =>
//             {
//                 _logger.LogInformation("Fee added {RequestId}", ctx.Message.RequestId);
//                 ctx.Saga.Fees.Add(ctx.Message);
//             })
//             .TransitionTo(ProcessBookingRefund)
//         );
//     }    
//     private void BookingCommittedHandler()
//     {
//         // Event(() => BookingCommit, x =>
//         //     x.CorrelateById(r => r.Message.RequestId));
//         //
//         // During(Submitted, Ignore(BookingCommit));
//         // During(Rejected, Ignore(BookingCommit));
//         //
//         // DuringAny(When(BookingCommit)
//         //     .Then(ctx =>
//         //     {
//         //         _logger.LogInformation("Booking refunded {RequestId}", ctx.Message.RequestId);
//         //     })
//         //     .TransitionTo(BookingRefunded)
//         // );
//     }
//
//     private void SubmittedHandler()
//     {
//         Event(() => RequestSubmitted, x =>
//             x.CorrelateById(r => r.Message.Id));
//
//         Initially(When(RequestSubmitted)
//             .Then(ctx =>
//             {
//                 _logger.LogInformation("OrderStateMachine Initially");
//                 ctx.Saga.CorrelationId = ctx.Message.Id;
//                 ctx.Saga.RefundDate = DateTime.UtcNow;
//             })
//             .TransitionTo(Submitted)
//         );
//         During(Submitted, Ignore(RequestSubmitted));
//         During(Submitted, Ignore(ChangedFlight));
//
//         During(Submitted, When(CompletedByFinance)
//             .If(rf => rf.Message.Accepted,
//                 x => x.TransitionTo(Accepted))
//             .If(rf => !rf.Message.Accepted,
//                 x => x.TransitionTo(Rejected))
//         );
//     }
// }