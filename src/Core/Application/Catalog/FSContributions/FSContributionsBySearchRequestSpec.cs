using FSH.WebApi.Domain.Catalog.Fund;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.FSContributions;
public class FSContributionsBySearchRequestSpec: EntitiesByPaginationFilterSpec<FSContribution,FSContributionDto> 
{
    public FSContributionsBySearchRequestSpec(SearchFSContributionsRequest request) : base(request)
    {
        //request.HasOrderBy
       Query 
       .Include(p => p.FinSupport)
       .Include(p => p.Native)
       .ThenInclude(p => p.RuralGov)
       .OrderByDescending(p=>p.FinSupport.Begin,!request.HasOrderBy())
       .ThenBy(p=>p.FinSupport.Name,!request.HasOrderBy())
       .ThenBy(p => p.Date, !request.HasOrderBy())
       .ThenBy(p => p.Native.Name, !request.HasOrderBy())
       
       .Where(p => p.NativeId.Equals(request.NativeId), request.NativeId.HasValue)
       .Where(p => p.FinSupportId.Equals(request.FinSupportId), request.FinSupportId.HasValue)
       .Where(p => p.Date >= request.DateStart!.Value.ToUniversalTime() && p.Date <= request.DateEnd!.Value.ToUniversalTime(), request.DateStart.HasValue && request.DateEnd.HasValue)
       .Where(p => p.Native.Name.Contains(request.FIO) || p.Native.Surname.Contains(request.FIO)
       || (!string.IsNullOrEmpty(p.Native.MiddleName) && p.Native.MiddleName.Contains(request.FIO)) 
       || p.FinSupport.Name.Contains(request.FIO)
       || p.Summa.ToString().Contains(request.FIO)
       || (p.Description!.Contains(request.FIO) && !string.IsNullOrEmpty(p.Description)),!string.IsNullOrEmpty(request.FIO));
    }
}
