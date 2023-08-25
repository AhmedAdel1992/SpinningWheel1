using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Common.DependencyInjection.Interfaces;
using Common.Entities;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Dto;
using Domain.Extensions.Models;
using Domain.Extensions.Specification;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class RewardRepository : BaseRepository<Reward>, IRewardRepository, IScoped
{
    private readonly AppDbContext _dbContext;

    public RewardRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsNameExisted(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.Rewards.AnyAsync(sw => sw.RewardName == name);
    }

    public async Task<bool> IsIdExisted(string id, CancellationToken cancellationToken)
    {
        return await _dbContext.Rewards.AnyAsync(sw => sw.Id == id, cancellationToken);
    }

    public async Task<PaginationResponse<RewardDto>> GetAll(PaginationFilter request,
   CancellationToken cancellationToken) =>
   await this.PaginatedListAsync(new GetAllRewardsSpec(request),
       request.PageNumber,
       request.PageSize, cancellationToken);
}

public class GetAllRewardsSpec : EntitiesByPaginationFilterSpec<Reward, RewardDto>
{
    public GetAllRewardsSpec(PaginationFilter request)
        : base(request)
    {
        Query
            .OrderBy(c => c.RewardName, !request.HasOrderBy());
    }
}