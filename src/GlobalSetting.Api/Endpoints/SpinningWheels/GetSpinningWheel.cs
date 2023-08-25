using Application.SpinningWheels.Queries;
using Domain.Dto;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.SpinningWheels;

public class GetSpinningWheel : MyEndpoint<GetSpinningWheelByIdQuery, SpinningWheelDto>
{
    public GetSpinningWheel(IMediator mediator) : base(mediator)
    {
    }

    public override void Configure()
    {
        Get("/spinningwheel/{id}");
        AllowAnonymous();
    }
}