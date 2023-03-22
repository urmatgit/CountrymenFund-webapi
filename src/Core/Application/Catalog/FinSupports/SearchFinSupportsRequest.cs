using FSH.WebApi.Domain.Catalog.Fund;

namespace FSH.WebApi.Application.Catalog.FinSupports;

public class SearchFinSupportsRequest : PaginationFilter, IRequest<PaginationResponse<FinSupportDto>>
{
}

public class FinSupportsBySearchRequestSpec : EntitiesByPaginationFilterSpec<FinSupport, FinSupportDto>
{
    public FinSupportsBySearchRequestSpec(SearchFinSupportsRequest request)
        : base(request) =>
        Query
        .OrderByDescending(c => c.Begin, !request.HasOrderBy())
        .ThenBy(c => c.Name, !request.HasOrderBy());
}

public class SearchFinSupportsRequestHandler : IRequestHandler<SearchFinSupportsRequest, PaginationResponse<FinSupportDto>>
{
    private readonly IReadRepository<FinSupport> _repository;

    public SearchFinSupportsRequestHandler(IReadRepository<FinSupport> repository) => _repository = repository;

    public async Task<PaginationResponse<FinSupportDto>> Handle(SearchFinSupportsRequest request, CancellationToken cancellationToken)
    {
        var spec = new FinSupportsBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}