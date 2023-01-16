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
public class RuralGovConfiguration : IEntityTypeConfiguration<RuralGov>
{
    public void Configure(EntityTypeBuilder<RuralGov> builder)
    {
        builder.IsMultiTenant();
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(256);
            


    }
}
