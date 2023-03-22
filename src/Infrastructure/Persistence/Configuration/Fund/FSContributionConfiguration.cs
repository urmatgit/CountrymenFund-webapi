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
public class FSContributionConfiguration : IEntityTypeConfiguration<FSContribution>
{
    public void Configure(EntityTypeBuilder<FSContribution> builder)
    {
        builder.IsMultiTenant();
        builder.Property(c => c.Summa).IsRequired();
        builder.Property(d=>d.Date).IsRequired();
    }
}
