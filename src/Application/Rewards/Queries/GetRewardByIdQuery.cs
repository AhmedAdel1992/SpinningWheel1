using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.SpinningWheels.Queries;
using Domain.Dto;
using LanguageExt.Common;
using Mapster;

namespace Application.Rewards.Queries;

public class GetRewardByIdQuery : IQuery<RewardDto>
{
    public string Id { get; set; }
}

public class GetByIdQueryHandler : IQueryHandler<GetRewardByIdQuery, RewardDto>
{
    private readonly IRewardRepository _rewardRepository;
    public GetByIdQueryHandler(IRewardRepository invoiceRepository) => _rewardRepository = invoiceRepository;

    public async Task<Result<RewardDto>> Handle(GetRewardByIdQuery request, CancellationToken cancellationToken)
    {
        var reward = await _rewardRepository.GetByIdAsync(request.Id);
        return reward.Adapt<RewardDto>();
    }
}
