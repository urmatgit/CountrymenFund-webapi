using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Years;
public class SearchYearsRequest: PaginationFilter,IRequest<PaginationResponse<YearDto>>
{

}
public class YearsBySearchRequestSpec: EntitiesByPaginationFilterSpec<Year, YearDto>
{
    public YearsBySearchRequestSpec(SearchYearsRequest request) : base(request)
        => Query.OrderBy(c => c.year, !request.HasOrderBy());
}
public class SearchYearsRequestHandler : IRequestHandler<SearchYearsRequest, PaginationResponse<YearDto>>
{
    private readonly IReadRepository<Year> _readRepository;
    public SearchYearsRequestHandler(IReadRepository<Year> readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<PaginationResponse<YearDto>> Handle(SearchYearsRequest request, CancellationToken cancellationToken)
    {
        var spec = new YearsBySearchRequestSpec(request);
        return await _readRepository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}
