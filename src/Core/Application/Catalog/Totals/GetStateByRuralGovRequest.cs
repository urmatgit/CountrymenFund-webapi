using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Domain.Catalog.Fund;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FSH.WebApi.Application.Common.Models;
using FSH.WebApi.Application.Identity.Users;

namespace FSH.WebApi.Application.Catalog.Totals;
public class GetStateByRuralGovRequest : PaginationFilter, IRequest<PaginationResponse<TotalWithMonths>>
{
    public DefaultIdType? YearId { get; set; }
    
}
public class GetStateByRuralGovRequestSpec : EntitiesByPaginationFilterSpec<Contribution, TotalWithMonths>
{
    public GetStateByRuralGovRequestSpec(GetStateByRuralGovRequest request) : base(request)
    => Query
        .Include(p => p.Year)
        .Include(p => p.Native)
        .ThenInclude(p => p.RuralGov)
        .OrderByDescending(o => o.Year.year,!request.HasOrderBy())
        .ThenBy(o => o.Native.RuralGov.Name, !request.HasOrderBy())
        .Where(p=>p.YearId== request.YearId,request.YearId.HasValue)
      //  .Where(p=>p.Native.RuralGovId==request.RuralGovId,!request.RuralGovId.HasValue)
        ;
}
public class GetStateByRuralGovRequestHandler : IRequestHandler<GetStateByRuralGovRequest, PaginationResponse<TotalWithMonths>>
{
    
    private readonly IDapperRepository dapperRepository;
    private readonly IStringLocalizer<GetStateByRuralGovRequestHandler> stringLocalizer;

    public GetStateByRuralGovRequestHandler( IStringLocalizer<GetStateByRuralGovRequestHandler> stringLocalizer, IDapperRepository dapperRepository)
    {
        
        this.stringLocalizer = stringLocalizer;
        this.dapperRepository = dapperRepository;
    }
    
    public async Task<PaginationResponse<TotalWithMonths>> Handle(GetStateByRuralGovRequest request, CancellationToken cancellationToken)
    {
        var totaByNative = new GetTotalByNativeData(dapperRepository, stringLocalizer, (PaginationFilter)request);
        IQueryable<Contribution> expression = dapperRepository.GetQueryable<Contribution>()
            .Include(p => p.Year)
            .Include(p => p.Native)
            .ThenInclude(p => p.RuralGov)
            .OrderByDescending(o => o.Year.year)
            .ThenBy(o => o.Native.RuralGov.Name);
        if (request.YearId.HasValue)
        {
            expression=expression.Where(p => p.YearId == request.YearId);
        }
            
        var query = await totaByNative.GetListByRuralGovs(expression,  cancellationToken);
        return new PaginationResponse<TotalWithMonths>(query, query.Count(), request.PageNumber, request.PageSize);
    }
}