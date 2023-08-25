using System;
using System.Diagnostics;
using Common.DependencyInjection.Interfaces;
using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Persistence.ValueGenerators;

namespace Persistence.Configuration.Base;

public abstract class BaseEntityConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity<TId>
{
    public abstract void EntityConfigure(EntityTypeBuilder<TEntity> builder);

    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        var generator = GetGenerator();
        if (generator is not null)
            builder.Property(e => e.Id)
                .HasValueGenerator(generator)
                .ValueGeneratedOnAdd();
        EntityConfigure(builder);
    }

    Type GetGenerator() => typeof(TId) switch
    {
        _ when typeof(TId) == typeof(Guid) => typeof(SeqGuidIdValueGenerator),
        _ when typeof(TId) == typeof(string) => typeof(SeqIdValueGenerator),
        _ => null
    };
}