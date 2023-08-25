using System;
using Common.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.Configuration.Base;

namespace Persistence.Configuration
{
    public class ExtraDataConfiguration : BaseEntityConfiguration<ExtraData, string>
    {
        public override void EntityConfigure(EntityTypeBuilder<ExtraData> builder)
        {
            builder.Property(s => s.ExtraDataName).IsRequired();
        }
    }

}
