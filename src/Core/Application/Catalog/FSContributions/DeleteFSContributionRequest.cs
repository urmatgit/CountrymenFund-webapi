using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.FSContributions;
public class DeleteFSContributionRequest: IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public DeleteFSContributionRequest(DefaultIdType id)
    {
        Id = id;
    }
}
public class DeleteFSContributionRequestHandler : IRequestHandler<DeleteFSContributionRequest, DefaultIdType>
{
    private readonly IRepositoryWithEvents<FSContribution> _repository;
    private readonly IStringLocalizer L;
    public DeleteFSContributionRequestHandler(IRepositoryWithEvents<FSContribution> repository, IStringLocalizer<DeleteFSContributionRequestHandler> l)
    {
        _repository = repository;
        L = l;
    }

    public async Task<DefaultIdType> Handle(DeleteFSContributionRequest request, CancellationToken cancellationToken)
    {
         var contr=await _repository.GetByIdAsync(request.Id);
        _ = contr ?? throw new NotFoundException(L["FSContribution {0} Not Found.", request.Id]);
        await _repository.DeleteAsync(contr,cancellationToken);
        return contr.Id;
    }
}
