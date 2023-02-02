using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class GetRuralGovsSummRequestSpec: Specification<Contribution,ContributionDto >
{
    public GetRuralGovsSummRequestSpec(DateTime begin, DateTime end)
        => Query.Include(x => x.Native)
        .ThenInclude(x => x.RuralGov)
        .Where(x => x.Date >= begin && x.Date <= end);
}
