
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
public class YearConfiguration : IEntityTypeConfiguration<Year>
{
    public void  Configure(EntityTypeBuilder<Year> builder)
    {
        builder.IsMultiTenant();
        builder.Property(y => y.year)
            .IsRequired();
            
    }
}
