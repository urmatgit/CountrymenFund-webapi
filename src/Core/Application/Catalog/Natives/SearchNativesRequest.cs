using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class SearchNativesRequest: PaginationFilter,IRequest<PaginationResponse<NativeDto>>
{
    public Guid? RuralGovId { get; set; }
    
}

public class SearchNativesRequestHandle : IRequestHandler<SearchNativesRequest, PaginationResponse<NativeDto>>
{
    private readonly IReadRepository<Native> _repository;
    public SearchNativesRequestHandle(IReadRepository<Native> repository)
    {
        _repository = repository;
    }

    public async Task<PaginationResponse<NativeDto>> Handle(SearchNativesRequest request, CancellationToken cancellationToken)
    {
        if (request.HasOrderBy())
        {
            for (int i = 0; i < request.OrderBy.Length; i++)
            {
                var orderby = request.OrderBy[i];
                switch (orderby)
                {
                    
                    case string s when s.StartsWith("RuralGovName"):
                        orderby = orderby.Replace("RuralGovName", "RuralGov.Name"); break;


                }
                request.OrderBy[i] = orderby;

            }
        }
        var spec =new NativesBySearchRequestWithRuralGovsSpec(request);
         return await _repository.PaginatedListAsync(spec,request.PageNumber,request.PageSize,cancellationToken);
    }
}