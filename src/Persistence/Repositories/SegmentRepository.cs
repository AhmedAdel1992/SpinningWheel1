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

public class SegmentRepository : BaseRepository<Segment>, ISegmentRepository, IScoped
{
    private readonly AppDbContext _dbContext;

    public SegmentRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsNameExisted(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.Segments.AnyAsync(sw => sw.Label == name);
    }

    public async Task<bool> IsIdExisted(string id, CancellationToken cancellationToken)
    {
        return await _dbContext.Segments.AnyAsync(sw => sw.Id == id, cancellationToken);
    }

    public async Task<PaginationResponse<SegmentDto>> GetAll(PaginationFilter request,
   CancellationToken cancellationToken) =>
   await this.PaginatedListAsync(new GetAllSegmentsSpec(request),
       request.PageNumber,
       request.PageSize, cancellationToken);
}

public class GetAllSegmentsSpec : EntitiesByPaginationFilterSpec<Segment, SegmentDto>
{
    public GetAllSegmentsSpec(PaginationFilter request)
        : base(request)
    {
        Query
            .OrderBy(c => c.Label, !request.HasOrderBy());
    }
}