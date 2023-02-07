using FSH.WebApi.Domain.Catalog.Fund;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class ContributionsBySearchRequestSpec: EntitiesByPaginationFilterSpec<Contribution,ContributionDto> 
{
    public ContributionsBySearchRequestSpec(SearchContributionsRequest request) : base(request)
    {
        //request.HasOrderBy
       Query 
       .Include(p => p.Year)
       .Include(p => p.Native)
       .ThenInclude(p => p.RuralGov)
       .OrderByDescending(p=>p.Year.year,!request.HasOrderBy())
       .ThenBy(p=>p.Month,!request.HasOrderBy())
       .ThenBy(p=>p.Native.RuralGov.Name,!request.HasOrderBy())
       .ThenBy(p => p.Native.Name, !request.HasOrderBy())
       .Where(p => p.YearId.Equals(request.YearId), request.YearId.HasValue)
       .Where(p => p.NativeId.Equals(request.NativeId), request.NativeId.HasValue)
       .Where(p => p.Native.RuralGovId.Equals(request.RuralGovId), request.RuralGovId.HasValue)
       .Where(p => p.Month == request.Month, request.Month.HasValue)
       .Where(p => p.Date >= request.DateStart!.Value.ToUniversalTime() && p.Date <= request.DateEnd!.Value.ToUniversalTime(), request.DateStart.HasValue && request.DateEnd.HasValue)
       .Where(p => p.Native.Name.Contains(request.FIO) || p.Native.Surname.Contains(request.FIO)
       || (!string.IsNullOrEmpty(p.Native.MiddleName) && p.Native.MiddleName.Contains(request.FIO)), !string.IsNullOrEmpty(request.FIO))
       .Where(p => p.Native.Name.Contains(request.Keyword) || p.Native.Surname.Contains(request.Keyword)
       || (!string.IsNullOrEmpty(p.Native.MiddleName) && p.Native.MiddleName.Contains(request.Keyword)), !string.IsNullOrEmpty(request.Keyword))
       .Where(p => p.Native.RuralGov.Name.Contains(request.Keyword) || p.Native.RuralGov.Description.Contains(request.Keyword), !string.IsNullOrEmpty(request.Keyword));

    }
}
