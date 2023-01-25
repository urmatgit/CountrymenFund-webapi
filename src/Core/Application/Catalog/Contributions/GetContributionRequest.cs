using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class GetContributionRequest: IRequest<ContributionDto>
{
    public DefaultIdType Id { get; set; }
    public GetContributionRequest(DefaultIdType id) => Id = id;
}
public class  ContributionByIdSpec: Specification<Contribution,ContributionDto>, ISingleResultSpecification
{
    public ContributionByIdSpec(DefaultIdType id) => Query.Where(p => p.Id == id);
}
public class GetContributionRequestHandler : IRequestHandler<GetContributionRequest, ContributionDto>
{
    private readonly IReadRepository<Contribution> _repository;
    private readonly IStringLocalizer _L;
    public GetContributionRequestHandler(IReadRepository<Contribution> repository, IStringLocalizer<GetContributionRequestHandler> l)
    {
        _repository = repository;
        _L = l;
    }

    public async Task<ContributionDto> Handle(GetContributionRequest request, CancellationToken cancellationToken)
    
        => await _repository.GetBySpecAsync((ISpecification<Contribution,ContributionDto>) new ContributionByIdSpec(request.Id),cancellationToken)
        ?? throw new NotFoundException(_L["Contribution {0} Not Found.",request.Id]);
        
    
}