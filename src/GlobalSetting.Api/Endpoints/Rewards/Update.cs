using Application.Rewards.Commands;
using Domain.Dto;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.Rewards;

public class Update : MyEndpoint<UpdateRewardCommand, RewardDto>
{
    public Update(IMediator mediator) : base(mediator)
    {
    }

    public override void Configure()
    {
        Put("/reward/");
        AllowAnonymous();
    }
}