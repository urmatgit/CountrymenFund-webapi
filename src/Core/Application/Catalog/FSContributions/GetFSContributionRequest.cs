using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.FSContributions;
public class GetFSContributionRequest: IRequest<FSContributionDto>
{
    public DefaultIdType Id { get; set; }
    public GetFSContributionRequest(DefaultIdType id) => Id = id;
}
public class  FSContributionByIdSpec: Specification<FSContribution,FSContributionDto>, ISingleResultSpecification
{
    public FSContributionByIdSpec(DefaultIdType id) => Query.Where(p => p.Id == id);
}
public class GetFSContributionRequestHandler : IRequestHandler<GetFSContributionRequest, FSContributionDto>
{
    private readonly IReadRepository<FSContribution> _repository;
    private readonly IStringLocalizer _L;
    public GetFSContributionRequestHandler(IReadRepository<FSContribution> repository, IStringLocalizer<GetFSContributionRequestHandler> l)
    {
        _repository = repository;
        _L = l;
    }

    public async Task<FSContributionDto> Handle(GetFSContributionRequest request, CancellationToken cancellationToken)
    
        => await _repository.GetBySpecAsync((ISpecification<FSContribution,FSContributionDto>) new FSContributionByIdSpec(request.Id),cancellationToken)
        ?? throw new NotFoundException(_L["FSContribution {0} Not Found.",request.Id]);
        
    
}