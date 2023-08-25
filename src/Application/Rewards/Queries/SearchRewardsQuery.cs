using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Rewards.Queries;
using Domain.Dto;
using Domain.Extensions.Models;
using LanguageExt.Common;

namespace Application.Rewards.Queries;

public class SearchRewardsQuery : PaginationFilter, IQuery<PaginationResponse<RewardDto>>
{
}

public class SearchInvoicesQueryHandler : IQueryHandler<SearchRewardsQuery, PaginationResponse<RewardDto>>
{
    private readonly IRewardRepository _rewardRepoistory;

    public SearchInvoicesQueryHandler(IRewardRepository invoiceRepository) => _rewardRepoistory = invoiceRepository;

    public async Task<Result<PaginationResponse<RewardDto>>> Handle(SearchRewardsQuery request,
        CancellationToken cancellationToken) =>
        await _rewardRepoistory
            .GetAll(request, cancellationToken);
}
