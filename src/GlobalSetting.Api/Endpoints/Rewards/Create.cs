using Application.Rewards.Commands;
using Domain.Dto;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.Rewards
{
    public class Create : MyEndpoint<CreateRewardCommand, RewardDto>
    {
        public Create(IMediator mediator) : base(mediator)
        {
        }

        public override void Configure()
        {
            Post("/reward");
            AllowAnonymous();
        }
    }

}
