using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Years;
public class GetYearRequest: IRequest<YearDto>
{
    public DefaultIdType Id { get; set; }
    public GetYearRequest(DefaultIdType id)
    {
        Id = id;
    }
}
public class GetYearRequestHandler : IRequestHandler<GetYearRequest, YearDto>
{
    private readonly IReadRepository<Year> _repository;
    private readonly IStringLocalizer _l;
    public GetYearRequestHandler(IReadRepository<Year> repository, IStringLocalizer<GetYearRequestHandler> l)
    {
        _repository = repository;
        _l = l;
    }

    public async Task<YearDto> Handle(GetYearRequest request, CancellationToken cancellationToken)
    => await _repository.GetBySpecAsync((ISpecification<Year, YearDto>)new YearByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(_l["Year {0} Not Found.", request.Id]);
}
        