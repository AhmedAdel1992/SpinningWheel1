using Application.Rewards.Commands;
using Application.Rewards.Queries;
using Domain.Dto;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.Rewards;

public class GetReward : MyEndpoint<GetRewardByIdQuery, RewardDto>
{
    public GetReward(IMediator mediator) : base(mediator)
    {
    }

    public override void Configure()
    {
        Get("/reward/{id}");
        AllowAnonymous();
    }
}