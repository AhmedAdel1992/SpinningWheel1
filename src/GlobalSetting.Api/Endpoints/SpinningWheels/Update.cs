using Application.SpinningWheels.Commands;
using Domain.Dto;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.SpinningWheels;

public class Update : MyEndpoint<UpdateSpinningWheelCommand, SpinningWheelDto>
{
    public Update(IMediator mediator) : base(mediator)
    {
    }

    public override void Configure()
    {
        Put("/spinningwheel/");
        AllowAnonymous();
    }
}