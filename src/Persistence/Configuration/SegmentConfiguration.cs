using System;
using Common.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.Configuration.Base;

namespace Persistence.Configuration
{
    public class SegmentConfiguration : BaseEntityConfiguration<Segment, string>
    {
        public override void EntityConfigure(EntityTypeBuilder<Segment> builder)
        {
            builder.Property(s => s.Label).IsRequired();
        }
    }
    
}
