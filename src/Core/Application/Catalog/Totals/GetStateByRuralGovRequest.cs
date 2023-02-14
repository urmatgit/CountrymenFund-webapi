using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Totals;
public class GetStateByRuralGovRequest : PaginationFilter, IRequest<PaginationResponse<TotalByRuralGovDto>>
{
    public DefaultIdType? YearId { get; set; }
    public DefaultIdType? RuralGovId { get; set; }
}
public class GetStateByRuralGovRequestSpec : EntitiesByPaginationFilterSpec<Contribution, TotalByRuralGovDto>
{
    public GetStateByRuralGovRequestSpec(GetStateByRuralGovRequest request) : base(request)
    => Query
        .Include(p => p.Year)
        .Include(p => p.Native)
        .ThenInclude(p => p.RuralGov)
        .OrderByDescending(o => o.Year.year, !request.HasOrderBy())
        .ThenBy(o => o.Native.RuralGov.Name, !request.HasOrderBy())
        ;
}
public class GetStateByRuralGovRequestHandler : IRequestHandler<GetStateByRuralGovRequest, PaginationResponse<TotalByRuralGovDto>>
{
    private readonly IReadRepository<Contribution> contributionRepository;
    private readonly IStringLocalizer<GetStateByRuralGovRequestHandler> stringLocalizer;

    public GetStateByRuralGovRequestHandler(IReadRepository<Contribution> contributionRepository, IStringLocalizer<GetStateByRuralGovRequestHandler> stringLocalizer)
    {
        this.contributionRepository = contributionRepository;
        this.stringLocalizer = stringLocalizer;
    }
    public Task<PaginationResponse<TotalByRuralGovDto>> Handle(GetStateByRuralGovRequest request, CancellationToken cancellationToken)
    {
        var expressein = (new GetStateByRuralGovRequestSpec(request)).WhereExpressions;
         var result= expressein
            .GroupBy(g=>g)
    }
}