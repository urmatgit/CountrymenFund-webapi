using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class SearchContributionsRequest: PaginationFilter,IRequest<PaginationResponse<ContributionDto>>
{
    public DefaultIdType? YearId { get; set; }
    public string FIO { get; set; }

}
public class SearchContributionsRequestHandler: IRequestHandler<SearchContributionsRequest, PaginationResponse<ContributionDto>>
{
    private readonly IReadRepository<Contribution> _repository;
    public SearchContributionsRequestHandler(IReadRepository<Contribution> repository)
    {
        _repository = repository;
    }

    public async Task<PaginationResponse<ContributionDto>> Handle(SearchContributionsRequest request, CancellationToken cancellationToken)
    {
        var spec = new ContributionsBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}
