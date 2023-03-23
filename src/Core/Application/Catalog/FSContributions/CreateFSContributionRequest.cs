using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.FSContributions;
public class CreateFSContributionRequest:FSContributionDto,  IRequest<DefaultIdType>
{
}
public class CreateFSContributionRequestHandler : IRequestHandler<CreateFSContributionRequest, DefaultIdType>
{
    private readonly IRepositoryWithEvents<FSContribution> _repository;
    private readonly IStringLocalizer _l;
    public CreateFSContributionRequestHandler(IRepositoryWithEvents<FSContribution> repository, IStringLocalizer<CreateFSContributionRequestHandler> l)
    {
        _repository = repository;
        _l = l;
    }

    public async Task<DefaultIdType> Handle(CreateFSContributionRequest request, CancellationToken cancellationToken)
    {
        var FSContribution = new FSContribution() {
            NativeId = request.NativeId,
            
            Summa = request.Summa,
            FinSupportId=request.FinSupportId,
            Date = request.Date.Value.ToUniversalTime(),
            Description=request.Description
        };
        await _repository.AddAsync(FSContribution,cancellationToken);
        return FSContribution.Id;

    }
}