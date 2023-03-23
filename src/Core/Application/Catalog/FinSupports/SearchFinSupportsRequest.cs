using FSH.WebApi.Domain.Catalog.Fund;

namespace FSH.WebApi.Application.Catalog.FinSupports;

public class SearchFinSupportsRequest : PaginationFilter, IRequest<PaginationResponse<FinSupportDto>>
{
    public bool? IsCompleted { get; set; }=null;
}

public class FinSupportsBySearchRequestSpec : EntitiesByPaginationFilterSpec<FinSupport, FinSupportDto>
{
    public FinSupportsBySearchRequestSpec(SearchFinSupportsRequest request)
        : base(request) =>
        Query
        .OrderByDescending(c => c.Begin, !request.HasOrderBy())
        .ThenBy(c => c.Name, !request.HasOrderBy())
        .Where(c=>(request.IsCompleted.Value? c.End!=null: c.End==null) ,request.IsCompleted.HasValue);
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