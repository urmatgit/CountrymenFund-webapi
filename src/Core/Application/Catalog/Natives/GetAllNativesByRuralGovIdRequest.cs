using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class GetAllNativesByRuralGovIdRequest:IRequest<List<NativeDto>>
{

    public DefaultIdType? RuralGovId { get; set; }
    public GetAllNativesByRuralGovIdRequest(DefaultIdType? id)
    {
        RuralGovId = id;
    }
}
public class GetAllNativesByRuralGovIdRequestHandler : IRequestHandler<GetAllNativesByRuralGovIdRequest, List<NativeDto>>
{
    private readonly IReadRepository<Native> _repository;
    public GetAllNativesByRuralGovIdRequestHandler(IReadRepository<Native> readRepository)
    {
            _repository = readRepository;
    }
    public async Task<List<NativeDto>> Handle(GetAllNativesByRuralGovIdRequest request, CancellationToken cancellationToken)
    {
        var spec=new GetAllNativesWithRuralGovSpecification(request);
        return await _repository.ListAsync(spec,cancellationToken);
    }
}
public class GetAllNativesWithRuralGovSpecification : Specification<Native, NativeDto>
{
    public GetAllNativesWithRuralGovSpecification(GetAllNativesByRuralGovIdRequest request) 
    {
        Query.Include(p => p.RuralGov)
            .Where(p => p.RuralGovId.Equals(request.RuralGovId),request.RuralGovId.HasValue)
            .OrderBy(o => o.RuralGov.Name)
            .ThenBy(o => o.Name)
            .ThenBy(o => o.Surname);


    }
}

