using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class DeleteContributionRequest: IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public DeleteContributionRequest(DefaultIdType id)
    {
        Id = id;
    }
}
public class DeleteContributionRequestHandler : IRequestHandler<DeleteContributionRequest, DefaultIdType>
{
    private readonly IRepositoryWithEvents<Contribution> _repository;
    private readonly IStringLocalizer L;
    public DeleteContributionRequestHandler(IRepositoryWithEvents<Contribution> repository, IStringLocalizer<DeleteContributionRequestHandler> l)
    {
        _repository = repository;
        L = l;
    }

    public async Task<DefaultIdType> Handle(DeleteContributionRequest request, CancellationToken cancellationToken)
    {
         var contr=await _repository.GetByIdAsync(request.Id);
        _ = contr ?? throw new NotFoundException(L["Contribution {0} Not Found.", request.Id]);
        await _repository.DeleteAsync(contr,cancellationToken);
        return contr.Id;
    }
}
