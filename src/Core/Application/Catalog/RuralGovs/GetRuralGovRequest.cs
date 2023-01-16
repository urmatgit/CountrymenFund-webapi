using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.RuralGovs;
public class GetRuralGovRequest: IRequest<RuralGovDto>
{
    public DefaultIdType Id { get; set; }
    public GetRuralGovRequest(DefaultIdType id)
    {
        Id = id;
    }
}
public class RuralGovByIdSpec: Specification<RuralGov,RuralGovDto>,ISingleResultSpecification
{
    public RuralGovByIdSpec(DefaultIdType id) => Query.Where(q => q.Id == id);
}
public class GetRuralRequestHandler : IRequestHandler<GetRuralGovRequest, RuralGovDto>
{
    readonly    IRepository<RuralGov> _repository;
    readonly IStringLocalizer _t;
    public GetRuralRequestHandler(IRepository<RuralGov> repository, IStringLocalizer<GetRuralRequestHandler> t)
    {
        _repository = repository;
        _t = t;
    }

    public async Task<RuralGovDto> Handle(GetRuralGovRequest request, CancellationToken cancellationToken)
    => await _repository.GetBySpecAsync((ISpecification<RuralGov, RuralGovDto>)new RuralGovByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(_t["Rural goverment {0} Not Found.", request.Id]);
}