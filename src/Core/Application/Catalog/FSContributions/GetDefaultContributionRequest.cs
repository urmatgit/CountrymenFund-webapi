using FSH.WebApi.Application.Catalog.Years;
using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.FSContributions;
public class GetDefaultFSContributionRequest:  IRequest<FSContributionDto>
{
}
public class GetDefaultFSContributionRequestHander : IRequestHandler<GetDefaultFSContributionRequest, FSContributionDto>
{
    private readonly IReadRepository<Year> _yearReposity;
    public GetDefaultFSContributionRequestHander(IReadRepository<Year> yearReposity)
    {
        _yearReposity = yearReposity;
    }

    public async Task<FSContributionDto> Handle(GetDefaultFSContributionRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.Now;
        
        var contr = new FSContributionDto()
        {
         
            Date = now,
         
        };
        return contr;
    }
}
