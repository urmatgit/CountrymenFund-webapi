using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.RuralGovs;
public class SearchRuralGovsRequest: PaginationFilter, IRequest<PaginationResponse<RuralGovDto>>
{
}
public class RuralBySearchRequestSpec: EntitiesByPaginationFilterSpec<RuralGov, RuralGovDto>
{
    public RuralBySearchRequestSpec(SearchRuralGovsRequest requst)
        : base(requst) => Query.OrderBy(c => c.Name, !requst.HasOrderBy());
    
}
public class SearchRuralGovsRequestHandler : IRequestHandler<SearchRuralGovsRequest, PaginationResponse<RuralGovDto>>
{
    private readonly IRepository<RuralGov> _repository;
    public SearchRuralGovsRequestHandler(IRepository<RuralGov> repository)
    {
        _repository = repository;
    }

    public async Task<PaginationResponse<RuralGovDto>> Handle(SearchRuralGovsRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
