using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class DeleteNativeRequest: IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public DeleteNativeRequest(DefaultIdType id)=>Id= id;
}
public class DeleteNativeRequestHandler : IRequestHandler<DeleteNativeRequest, DefaultIdType>
{
    readonly IRepositoryWithEvents<Native> _reposity;
    readonly IStringLocalizer _t;
    public DeleteNativeRequestHandler(IRepositoryWithEvents<Native> repository, IStringLocalizer<DeleteNativeRequestHandler> l)
        => (_reposity, _t) = (repository, l);

    
    public async Task<DefaultIdType> Handle(DeleteNativeRequest request, CancellationToken cancellationToken)
    {
        var native=await _reposity.GetByIdAsync(request.Id);
        _=native ?? throw new NotFoundException(_t["Native {0} Not Found."]);
        await _reposity.DeleteAsync(native, cancellationToken);
       return request.Id
    }
}
