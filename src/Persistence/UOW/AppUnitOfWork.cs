using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.DependencyInjection.Interfaces;
using Common.Entities.Interfaces;
using Common.Logging;
using Domain.Abstractions;
using Domain.Entities.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Persistence.UOW;

public class AppUnitOfWork : IAppUnitOfWork, IScoped
{
    private readonly AppDbContext _context;
    private readonly IMediator _mediator;
    private readonly ILoggerAdapter<AppUnitOfWork> _logger;

    public AppUnitOfWork(AppDbContext context, IMediator mediator, ILoggerAdapter<AppUnitOfWork> logger)
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = TrackedEntities();
            var domainEvents = DomainEvents(entities);
            PreSaveChanges(entities);
            await _context.SaveChangesAsync(cancellationToken);
            PostSaveChanges(entities);
            await RaiseEntitiesEvents(domainEvents, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError(e, "Failed to save changes to database");
            throw;
        }
    }

    private async Task RaiseEntitiesEvents(List<IDomainEvent> domainEvents,
        CancellationToken cancellationToken)
    {
    

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }

    private  List<IDomainEvent> DomainEvents(List<EntityEntry> entities)
    {
        var domainEvents = entities
            .Select(e => e.Entity)
            .Where(e => e is IDomainEvent).Cast<Entity>()
            .SelectMany(s =>
            {
                var result = s.DomainEvents.ToList();
                s.ClearDomainEvents();
                return result;
            })
            .ToList();
        return domainEvents;
    }

    private List<EntityEntry> TrackedEntities()
    {
        return _context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted)
            .ToList();
    }

    private void PostSaveChanges(List<EntityEntry> entities)
    {
    }

    private void PreSaveChanges(List<EntityEntry> entities)
    {
        _context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .ToList()
            .ForEach(e =>
            {
                if ( e.Entity is ICreate createEntity  && e.State == EntityState.Added)
                {
                    createEntity.CreatedDate = DateTime.UtcNow;
                    createEntity.CreatedBy= "System";
                }
                if (e.Entity is IUpdate updatedEntity && e.State == EntityState.Modified)
                {
                    updatedEntity.ModifiedDate = DateTime.UtcNow;
                    updatedEntity.ModifiedBy= "System";
                }
            });
    }
}