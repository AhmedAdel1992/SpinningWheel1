using System;
using Common.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.Configuration.Base;

namespace Persistence.Configuration
{
    public class SpinningWheelConfiguration : BaseEntityConfiguration<SpinningWheel, string>
    {
        public override void EntityConfigure(EntityTypeBuilder<SpinningWheel> builder)
        {
            builder.Property(sw => sw.Name).IsRequired();

            builder
                .HasMany(sw => sw.Segments)
                .WithOne(s => s.SpinningWheel)
                .HasForeignKey(s => s.SpinningWheelId);
        }
    }
}
