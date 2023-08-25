using Domain.Abstractions.Repositories;
using Domain.Dto;
using Domain.Entities;
using Domain.Extensions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IRewardRepository : IRepository<Reward>
    {
        Task<bool> IsNameExisted(string name, CancellationToken cancellationToken);
        Task<bool> IsIdExisted(string id, CancellationToken cancellationToken);
        Task<PaginationResponse<RewardDto>> GetAll(PaginationFilter request, CancellationToken cancellationToken);
    }

}
