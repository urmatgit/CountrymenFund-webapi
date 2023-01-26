using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class CreateContributionRequest:ContributionDto,  IRequest<DefaultIdType>
{
}
public class CreateContributionRequestHandler : IRequestHandler<CreateContributionRequest, DefaultIdType>
{
    private readonly IRepositoryWithEvents<Contribution> _repository;
    private readonly IStringLocalizer _l;
    public CreateContributionRequestHandler(IRepositoryWithEvents<Contribution> repository, IStringLocalizer<CreateContributionRequestHandler> l)
    {
        _repository = repository;
        _l = l;
    }

    public async Task<DefaultIdType> Handle(CreateContributionRequest request, CancellationToken cancellationToken)
    {
        var contribution = new Contribution() {
            NativeId = request.NativeId,
            YearId = request.YearId,
            Summa = request.Summa,
            Month = request.Month,
            Date = request.Date.Value.ToUniversalTime(),
            Description=request.Description
        };
        await _repository.AddAsync(contribution,cancellationToken);
        return contribution.Id;

    }
}