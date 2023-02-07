using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class NativesBySearchRequestWithRuralGovsSpec: EntitiesByPaginationFilterSpec<Native,NativeDto>
{
    public NativesBySearchRequestWithRuralGovsSpec(SearchNativesRequest request) : base(request)
        => Query
        .Include(p => p.RuralGov)
        .OrderBy(p => p.Name,!request.HasOrderBy())
        .ThenBy(p=>p.Surname,!request.HasOrderBy())
        .ThenBy(p=>p.MiddleName,!request.HasOrderBy())
        .Where(p => p.RuralGovId.Equals(request.RuralGovId!.Value), request.RuralGovId.HasValue)
        .Where(p=>p.Rate>=request.MinimumRate!.Value,request.MinimumRate.HasValue)
        .Where(p=>p.Rate<=request.MaximumRate!.Value,request.MaximumRate.HasValue);


}
