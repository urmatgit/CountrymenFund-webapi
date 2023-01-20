using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class GetNativeRequest: IRequest<NativeDto>
{
    public DefaultIdType Id { get; set; }
    public GetNativeRequest(DefaultIdType id) => Id = id;
}
public class GetNativeRequestHandler : IRequestHandler<GetNativeRequest, NativeDto>
{
    private readonly IReadRepository<Native> _repository;
    private readonly IStringLocalizer _l;
    public GetNativeRequestHandler(IReadRepository<Native> repository, IStringLocalizer<GetNativeRequestHandler> l)
    {
        _repository = repository;
        _l = l;
    }

    public async Task<NativeDto> Handle(GetNativeRequest request, CancellationToken cancellationToken)
    => await _repository.GetBySpecAsync(
        (ISpecification<Native, NativeDto>)new NativeByIdWithRuralGovSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(_l["Native {0} Not Found.", request.Id]);
}
