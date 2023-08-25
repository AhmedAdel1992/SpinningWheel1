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

public class SpinningWheelRepository : BaseRepository<SpinningWheel>, ISpinningWheelRepository, IScoped
{
    private readonly AppDbContext _dbContext;

    public SpinningWheelRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> IsNameExisted(string name, CancellationToken cancellationToken)
    {
        return await _dbContext.SpinningWheels.AnyAsync(sw => sw.Name == name);
    }

    public async Task<bool> IsIdExisted(string id, CancellationToken cancellationToken)
    {
        return await _dbContext.SpinningWheels.AnyAsync(sw => sw.Id == id, cancellationToken);
    }

    public async Task<PaginationResponse<SpinningWheelDto>> GetAll(PaginationFilter request,
   CancellationToken cancellationToken) =>
   await this.PaginatedListAsync(new GetAllSpinningWheelsSpec(request),
       request.PageNumber,
       request.PageSize, cancellationToken);
}

public class GetAllSpinningWheelsSpec : EntitiesByPaginationFilterSpec<SpinningWheel, SpinningWheelDto>
{
    public GetAllSpinningWheelsSpec(PaginationFilter request)
        : base(request)
    {
        Query
            .Include(sw => sw.Segments)
            .OrderBy(c => c.Name, !request.HasOrderBy());
        
    }
}