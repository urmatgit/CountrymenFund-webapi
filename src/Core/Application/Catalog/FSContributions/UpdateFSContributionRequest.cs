using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.FSContributions;
public class UpdateFSContributionRequest :FSContributionDto , IRequest<DefaultIdType>
{
}
public class UpdateFSContributionRequestValidator: CreateFSContributionRequestValidator
{
    public UpdateFSContributionRequestValidator(IStringLocalizer<UpdateFSContributionRequestValidator> L) : base(L)
    {
    
    }
}
public class UpdateFSContributionRequestHandler : IRequestHandler<UpdateFSContributionRequest, DefaultIdType>
{
    private readonly IStringLocalizer _L;
    private readonly IRepositoryWithEvents<FSContribution> _repository;
    public UpdateFSContributionRequestHandler(IStringLocalizer<UpdateFSContributionRequestHandler> l, IRepositoryWithEvents<FSContribution> repository)
    {
        _L = l;
        _repository = repository;
    }

    public async Task<DefaultIdType> Handle(UpdateFSContributionRequest request, CancellationToken cancellationToken)
    {
        var contr=await _repository.GetByIdAsync(request.Id, cancellationToken);
        _= contr ?? throw new NotFoundException(_L["FSContribution {0} Not Found.",request.Id]);
        contr.Update(request.Summa,   request.Date, request.NativeId,request.Description, request.FinSupportId);
        await _repository.UpdateAsync(contr, cancellationToken);
        return request.Id;
            
    }
}