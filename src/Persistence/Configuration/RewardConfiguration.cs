using System;
using Common.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.Configuration.Base;

namespace Persistence.Configuration
{
    internal class RewardConfiguration : BaseEntityConfiguration<Reward, string>
    {
        public override void EntityConfigure(EntityTypeBuilder<Reward> builder)
        {
            builder
                .HasMany(r => r.Segments)
                .WithOne(s => s.Reward)
                .HasForeignKey(s => s.RewardId);

            builder
               .HasMany(r => r.ExtraDatas)
               .WithOne(ed => ed.Reward)
               .HasForeignKey(ed => ed.RewardId);
        }
    }
}
