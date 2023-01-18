using Finbuckle.MultiTenant.EntityFrameworkCore;
using FSH.WebApi.Domain.Catalog.Fund;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Infrastructure.Persistence.Configuration.Fund;
public class NativeConfiguration : IEntityTypeConfiguration<Native>
{
    public void Configure(EntityTypeBuilder<Native> builder)
    {
        builder.IsMultiTenant();
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(256);
        builder.Property(p => p.ImagePath).HasMaxLength(2048);
    }
}
