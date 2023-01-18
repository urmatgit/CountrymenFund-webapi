using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.RuralGovs;
public class DeleteRuralGovRequest : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public DeleteRuralGovRequest(DefaultIdType id) => Id = id;
}
public class DeleteRuralGovRequestHandler : IRequestHandler<DeleteRuralGovRequest, DefaultIdType>
{
    private readonly IRepositoryWithEvents<RuralGov> repository;
    private readonly IStringLocalizer<DeleteRuralGovRequestHandler> _t;
    public DeleteRuralGovRequestHandler(IRepositoryWithEvents<RuralGov> repository, IStringLocalizer<DeleteRuralGovRequestHandler> t)
    {
        this.repository = repository;
        _t = t;
    }

    public async Task<DefaultIdType> Handle(DeleteRuralGovRequest request, CancellationToken cancellationToken)
    {
        var ruralGov=await repository.GetByIdAsync(request.Id,cancellationToken);
        _ = ruralGov ?? throw new NotFoundException(_t["Rural goverment {0} Not Found."]);
        
        await repository.DeleteAsync(ruralGov,cancellationToken);
        return request.Id;
    }
}
