using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.HomePage;

public class SearchNewsPostRequest : PaginationFilter, IRequest<PaginationResponse<NewsPostDto>>
{

}

public class NewsPostBySearchRequestSpec : EntitiesByPaginationFilterSpec<NewsPost, NewsPostDto>
{
    public NewsPostBySearchRequestSpec(SearchNewsPostRequest request)
        : base(request) =>
        Query.OrderByDescending(c => c.CreatedOn, !request.HasOrderBy());
}

public class SearchNewsPostRequestHandler : IRequestHandler<SearchNewsPostRequest, PaginationResponse<NewsPostDto>>
{
    private readonly IReadRepository<NewsPost> _repository;

    public SearchNewsPostRequestHandler(IReadRepository<NewsPost> repository) => _repository = repository;

    public async Task<PaginationResponse<NewsPostDto>> Handle(SearchNewsPostRequest request, CancellationToken cancellationToken)
    {
        request.OrderBy = new string[] { };
        var spec = new NewsPostBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}

