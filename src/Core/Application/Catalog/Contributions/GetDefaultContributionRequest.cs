using FSH.WebApi.Application.Catalog.Years;
using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class GetDefaultContributionRequest:  IRequest<ContributionDto>
{
}
public class GetDefaultContributionRequestHander : IRequestHandler<GetDefaultContributionRequest, ContributionDto>
{
    private readonly IReadRepository<Year> _yearReposity;
    public GetDefaultContributionRequestHander(IReadRepository<Year> yearReposity)
    {
        _yearReposity = yearReposity;
    }

    public async Task<ContributionDto> Handle(GetDefaultContributionRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        var year =await  _yearReposity.GetBySpecAsync(new YearbyYearSpec(now.Year));
        var contr = new ContributionDto()
        {
            Month = (Months)now.Month,
            Date = now,
            YearId = year is not null? year.Id : Guid.Empty
        };
        return contr;
    }
}
