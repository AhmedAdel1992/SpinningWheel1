using Application.Rewards.Commands;
using Application.Rewards.Queries;
using Domain.Dto;
using Domain.Extensions.Models;
using MediatR;
using Wallet.Api.Endpoints.Base;

namespace Wallet.Api.Endpoints.Rewards;

public class Search : MyEndpoint<SearchRewardsQuery, PaginationResponse<RewardDto>>
{
    public Search(IMediator mediator) : base(mediator)
    {
    }

    public override void Configure()
    {
        Post("/reward/search");
        AllowAnonymous();
    }
}