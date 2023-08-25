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

public class ExtraDataRepository : BaseRepository<ExtraData>, IExtraDataRepository, IScoped
{
    private readonly AppDbContext _dbContext;

    public ExtraDataRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsNameExisted(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.ExtraDatas.AnyAsync(sw => sw.ExtraDataName == name);
    }

    public async Task<bool> IsIdExisted(string id, CancellationToken cancellationToken)
    {
        return await _dbContext.ExtraDatas.AnyAsync(sw => sw.Id == id, cancellationToken);
    }

    public async Task<PaginationResponse<ExtraDataDto>> GetAll(PaginationFilter request,
   CancellationToken cancellationToken) =>
   await this.PaginatedListAsync(new GetAllExtraDataSpec(request),
       request.PageNumber,
       request.PageSize, cancellationToken);
}

public class GetAllExtraDataSpec : EntitiesByPaginationFilterSpec<ExtraData, ExtraDataDto>
{
    public GetAllExtraDataSpec(PaginationFilter request)
        : base(request)
    {
        Query
            .OrderBy(c => c.ExtraDataName, !request.HasOrderBy());
    }
}