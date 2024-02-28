using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.RuralGovs;

public class GetllRuralGovsDtoRequest : IRequest<List<RuralGovDto>>
{
 
}

public class GetRuralGovsDtoRequestHandler : IRequestHandler<GetllRuralGovsDtoRequest, List<RuralGovDto>>
{
    private readonly IRepository<RuralGov> _repository;
    private readonly IStringLocalizer _t;

    public GetRuralGovsDtoRequestHandler(IRepository<RuralGov> repository, IStringLocalizer<GetRuralGovsDtoRequestHandler> localizer) =>
        (_repository, _t) = (repository, localizer);

    public async Task<List<RuralGovDto>> Handle(GetllRuralGovsDtoRequest request, CancellationToken cancellationToken) =>
        await _repository.ListAsync<RuralGovDto>( new RuralGovAllSpec(), cancellationToken)
        ?? throw new NotFoundException(_t["Entitys Not Found."]);
}
