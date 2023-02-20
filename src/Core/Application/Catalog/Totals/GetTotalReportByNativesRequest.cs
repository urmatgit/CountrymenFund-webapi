using Ardalis.Specification.EntityFrameworkCore;
using FSH.WebApi.Domain.Catalog.Fund;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        var expressein = dapperRepository.GetQueryable<Contribution>()
            .WithSpecification(new GetTotalReportByNativesRequestSpec(request));

        var query =await getSummColMonths(expressein)
                       .ToListAsync(cancellationToken);
        if (!request.HasOrderBy() && query!=null)
        {
            query = query.OrderBy(o => o.Year)
            .ThenBy(o => o.RuralGovName)
            .ThenBy(o => o.FIO)
            .ToList();
        }
        var total = new TotalByNative
        {
            Year = 0,
            FIO = stringLocalizer.GetString("Total"),
            RuralGovName= stringLocalizer.GetString("Total"),
            Village="",

            January = query.Sum(x => x.January),
            February = query.Sum(x => x.February),
            March = query.Sum(x => x.March),
            April = query.Sum(x => x.April),
            May = query.Sum(x => x.May),
            June = query.Sum(x => x.June),
            July = query.Sum(x => x.July),
            August = query.Sum(x => x.August),
            September = query.Sum(x => x.September),
            October = query.Sum(x => x.October),
            November = query.Sum(x => x.November),
            December = query.Sum(x => x.December),
            AllSumm = query.Sum(x => x.AllSumm),
            Style = "font-weight: bold;"
        };
        query.Add(total);
        return new PaginationResponse<TotalByNative>(query, query.Count(), request.PageNumber, request.PageSize);
    }
    private IQueryable<TotalByNative> getSummColMonths(IQueryable<Contribution> queryable)
    {
        //TotalWithMonths
        var query =
           queryable
           .GroupBy(contr =>
            new
            {
                contr.Year.year,
                //contr.Native.RuralGov.Name
                nativeid = contr.NativeId
                //, contr.Month
                //fio = 
            }).Select(x => new TotalByNative
            {
                // Month = x.Key.Month,
                FIO = x.Where(y => y.NativeId == x.Key.nativeid).Select(n => $"{n.Native.Name} {n.Native.Surname} {n.Native!.MiddleName}").FirstOrDefault(),
                Village = x.FirstOrDefault(y => y.NativeId == x.Key.nativeid).Native.Village ?? "",
                RuralGovName = x.FirstOrDefault(y => y.NativeId == x.Key.nativeid).Native.RuralGov.Name,
                Year = x.Key.year,
                January = x.Where(c => c.Month == Shared.Enums.Months.January).Sum(c => c.Summa),
                February = x.Where(c => c.Month == Shared.Enums.Months.February).Sum(c => c.Summa),
                March = x.Where(c => c.Month == Shared.Enums.Months.March).Sum(c => c.Summa),
                April = x.Where(c => c.Month == Shared.Enums.Months.April).Sum(c => c.Summa),
                May = x.Where(c => c.Month == Shared.Enums.Months.May).Sum(c => c.Summa),
                June = x.Where(c => c.Month == Shared.Enums.Months.June).Sum(c => c.Summa),
                July = x.Where(c => c.Month == Shared.Enums.Months.July).Sum(c => c.Summa),
                August = x.Where(c => c.Month == Shared.Enums.Months.August).Sum(c => c.Summa),
                September = x.Where(c => c.Month == Shared.Enums.Months.September).Sum(c => c.Summa),
                October = x.Where(c => c.Month == Shared.Enums.Months.October).Sum(c => c.Summa),
                November = x.Where(c => c.Month == Shared.Enums.Months.November).Sum(c => c.Summa),
                December = x.Where(c => c.Month == Shared.Enums.Months.December).Sum(c => c.Summa),
                AllSumm = x.Sum(c => c.Summa)

            });
            
        return query;
    }
}
