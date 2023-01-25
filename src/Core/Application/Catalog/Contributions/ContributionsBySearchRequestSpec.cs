using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class ContributionsBySearchRequestSpec: EntitiesByPaginationFilterSpec<Contribution,ContributionDto> 
{
    public ContributionsBySearchRequestSpec(SearchContributionsRequest request) : base(request)
        => Query
        .Include(p => p.Year)
        .Include(p => p.Native)
        .OrderBy(p => p.Native.Name)
        .Where(p => p.YearId.Equals(request.YearId), request.YearId.HasValue)
        .Where(p => p.Native.Name.Contains(request.FIO) || p.Native.Surname.Contains(request.FIO)
        || (!string.IsNullOrEmpty(p.Native.MiddleName) && p.Native.MiddleName.Contains(request.FIO)), !string.IsNullOrEmpty(request.FIO));

}
