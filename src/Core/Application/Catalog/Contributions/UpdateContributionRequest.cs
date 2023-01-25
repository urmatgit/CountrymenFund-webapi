using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class UpdateContributionRequest :ContributionDto , IRequest<DefaultIdType>
{
}
public class UpdateContributionRequestValidator: CreateContributionRequestValidator
{
    public UpdateContributionRequestValidator(IStringLocalizer<UpdateContributionRequestValidator> L) : base(L)
    {
    
    }
}
public class UpdateContributionRequestHandler : IRequestHandler<UpdateContributionRequest, DefaultIdType>
{
    private readonly IStringLocalizer _L;
    private readonly IRepositoryWithEvents<Contribution> _repository;
    public UpdateContributionRequestHandler(IStringLocalizer<UpdateContributionRequestHandler> l, IRepositoryWithEvents<Contribution> repository)
    {
        _L = l;
        _repository = repository;
    }

    public async Task<DefaultIdType> Handle(UpdateContributionRequest request, CancellationToken cancellationToken)
    {
        var contr=await _repository.GetByIdAsync(request.Id, cancellationToken);
        _= contr ?? throw new NotFoundException(_L["Contribution {0} Not Found.",request.Id]);
        contr.Update(request.Summa, request.Month, request.Date, request.NativeId, request.YearId,request.Description);
        await _repository.UpdateAsync(contr, cancellationToken);
        return request.Id;
            
    }
}