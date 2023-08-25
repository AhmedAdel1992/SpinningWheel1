using Application.Rewards.Commands;
using Domain.Dto;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.Rewards;

public class Delete : MyEndpoint<DeleteRewardCommand, RewardDto>
{
    public Delete(IMediator mediator) : base(mediator)
    {
    }

    public override void Configure()
    {
        Delete("/reward/{id}");
        AllowAnonymous();
    }
}