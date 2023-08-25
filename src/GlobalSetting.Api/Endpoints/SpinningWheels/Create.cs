using Application.SpinningWheels.Commands;
using Domain.Dto;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.SpinningWheels
{
    public class Create : MyEndpoint<CreateSpinningWheelCommand, SpinningWheelDto>
    {
        public Create(IMediator mediator) : base(mediator)
        {
        }

        public override void Configure()
        {
            Post("/spinningwheel");
            AllowAnonymous();
        }
    }
  
}
