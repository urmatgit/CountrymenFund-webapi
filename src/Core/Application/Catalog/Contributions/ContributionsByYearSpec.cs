using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class ContributionsByYearSpec: Specification<Contribution>
{
    public ContributionsByYearSpec(DefaultIdType yearId) => Query.Where(c => c.YearId == yearId);
    
}
