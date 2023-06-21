using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class FindContributionByParamSpec : SingleResultSpecification<Contribution>
{
    public FindContributionByParamSpec(Contribution contribution) => Query
        .Include(p => p.Year)
       .Include(p => p.Native)
        .Where(c => c.NativeId == contribution.NativeId && c.Date.Date == contribution.Date.Date && c.Month == contribution.Month && c.YearId == contribution.YearId && c.Summa == contribution.Summa);
    
}
