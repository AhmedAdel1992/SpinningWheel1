using Application.SpinningWheels.Commands;
using Domain.Dto;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.SpinningWheels;

public class Delete : MyEndpoint<DeleteSpinningWheelCommand, SpinningWheelDto>
{
    public Delete(IMediator mediator) : base(mediator)
    {
    }

    public override void Configure()
    {
        Delete("/spinningwheel/{id}");
        AllowAnonymous();
    }
}