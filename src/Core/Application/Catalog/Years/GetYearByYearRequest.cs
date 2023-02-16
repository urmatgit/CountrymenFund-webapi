using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Years;
public class GetYearByYearRequest : IRequest<YearDto>
{
    public int year { get; set; }
    public GetYearByYearRequest(int year)
    {
        this.year = year;
    }
}
public class GetYearByYearRequestHandler : IRequestHandler<GetYearByYearRequest, YearDto>
{
    private readonly IReadRepository<Year> _repository;
    private readonly IStringLocalizer _l;
    public GetYearByYearRequestHandler(IReadRepository<Year> repository, IStringLocalizer<GetYearByYearRequestHandler> l)
    {
        _repository = repository;
        _l = l;
    }

    public async Task<YearDto> Handle(GetYearByYearRequest request, CancellationToken cancellationToken)
    => await _repository.GetBySpecAsync((ISpecification<Year, YearDto>)new YearbyYearSpec(request.year), cancellationToken)
        ?? throw new NotFoundException(_l["Year {0} Not Found.", request.year]);
}
        