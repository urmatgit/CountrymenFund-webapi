using Ardalis.Specification.EntityFrameworkCore;
using FSH.WebApi.Domain.Catalog.Fund;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FSH.WebApi.Application.Catalog.Totals;
public class GetTotalReportByNativesRequest: PaginationFilter ,IRequest<PaginationResponse<TotalByNative>>
{
    public DefaultIdType? YearId { get; set; }
    public DefaultIdType? RuralGovId { get; set;}
}
public class GetTotalReportByNativesRequestSpec : EntitiesByPaginationFilterSpec<Contribution, TotalByNative>
{
    public GetTotalReportByNativesRequestSpec(GetTotalReportByNativesRequest request) : base(request)
    => Query
        .Include(p => p.Year)
        .Include(p => p.Native)
        .ThenInclude(p => p.RuralGov)
        //.OrderByDescending(o => o.Year.year, !request.HasOrderBy())
        //.ThenBy(o => o.Native.RuralGov.Name, !request.HasOrderBy())
        .Where(p => p.YearId == request.YearId, request.YearId.HasValue)
        .Where(p=>p.Native.RuralGovId==request.RuralGovId,request.RuralGovId.HasValue)
        ;
}
public class GetTotalReportByNativesRequestHandler : IRequestHandler<GetTotalReportByNativesRequest, PaginationResponse<TotalByNative>>
{
    private readonly IDapperRepository dapperRepository;
    private readonly IStringLocalizer<GetTotalReportByNativesRequestHandler> stringLocalizer;
    public GetTotalReportByNativesRequestHandler(IDapperRepository dapperRepository, IStringLocalizer<GetTotalReportByNativesRequestHandler> stringLocalizer)
    {
        this.dapperRepository = dapperRepository;
        this.stringLocalizer = stringLocalizer;
    }

    public async Task<PaginationResponse<TotalByNative>> Handle(GetTotalReportByNativesRequest request, CancellationToken cancellationToken)
    {
        var totaByNative = new GetTotalByNativeData(dapperRepository, stringLocalizer);
        var query = await totaByNative.GetListByNatives(new GetTotalReportByNativesRequestSpec(request), request.HasOrderBy(), cancellationToken);             
        return new PaginationResponse<TotalByNative>(query, query.Count(), request.PageNumber, request.PageSize);
    }
  
}
