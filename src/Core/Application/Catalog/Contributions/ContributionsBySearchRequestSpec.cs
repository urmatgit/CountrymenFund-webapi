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
        .ThenInclude(p=>p.RuralGov)
        .OrderBy(p=>p.Native.RuralGov.Name)
        .OrderBy(p => p.Native.Name)
        .Where(p => p.YearId.Equals(request.YearId), request.YearId.HasValue)
        .Where(p=>p.NativeId.Equals(request.NativeId),request.NativeId.HasValue)
        .Where(p=>p.Native.RuralGovId.Equals(request.RuralGovId),request.RuralGovId.HasValue)
        .Where(p=>p.Month==request.Month,request.Month.HasValue)
        .Where(p=>p.Date>=request.DateStart!.Value.ToUniversalTime() && p.Date<=request.DateEnd!.Value.ToUniversalTime(), request.DateStart.HasValue && request.DateEnd.HasValue)
        .Where(p => p.Native.Name.Contains(request.FIO) || p.Native.Surname.Contains(request.FIO)
        || (!string.IsNullOrEmpty(p.Native.MiddleName) && p.Native.MiddleName.Contains(request.FIO)), !string.IsNullOrEmpty(request.FIO));

}
