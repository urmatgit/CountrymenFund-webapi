using FSH.WebApi.Application.Catalog.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.NewsPosts;

public class GetNewsPostRequest : IRequest<NewsPostDto>
{
    public Guid Id { get; set; }

    public GetNewsPostRequest(Guid id) => Id = id;
}
public class NewsPostByIdSpec : Specification< NewsPost, NewsPostDto>, ISingleResultSpecification
{
    public NewsPostByIdSpec(Guid id) =>
        Query.Where(p => p.Id == id);
}
public class GetNewsPostRequestHandler : IRequestHandler<GetNewsPostRequest, NewsPostDto>
{
    private readonly IRepository<NewsPost> _repository;
    private readonly IStringLocalizer _t;

    public GetNewsPostRequestHandler(IRepository<NewsPost> repository, IStringLocalizer<GetNewsPostRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<NewsPostDto> Handle(GetNewsPostRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<NewsPost, NewsPostDto>)new NewsPostByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(_t["Entity {0} Not Found.", request.Id]);
}
