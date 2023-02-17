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
        //.OrderByDescending(o => o.Year.year, !request.HasOrderBy())
        //.ThenBy(o => o.Native.RuralGov.Name, !request.HasOrderBy())
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
    private IQueryable<TotalByRuralGovDto> getSummAllMonth(IQueryable<Contribution> queryable)
    {
        //TotalWithMonths
        var query =
           queryable
           .GroupBy(contr =>
            new
            {
                contr.Year.year,
                contr.Native.RuralGov.Name,
                contr.Month
                // fio = $"{contr.Native.Name} {contr.Native.Surname} {contr.Native.MiddleName} ({contr.Native.Village})"
            }).Select(x => new TotalByRuralGovDto
            {
                Month = x.Key.Month,
                RuralGovName = x.Key.Name,
                Year = x.Key.year,
                Summa = x.Sum(y => y.Summa)
            });
        return query;
    }
    private IQueryable<TotalWithMonths> getSummColMonths(IQueryable<Contribution> queryable)
    {
        //TotalWithMonths
        var query =
           queryable
           .GroupBy(contr =>
            new
            {
                contr.Year.year,
                contr.Native.RuralGov.Name
                //, contr.Month
                // fio = $"{contr.Native.Name} {contr.Native.Surname} {contr.Native.MiddleName} ({contr.Native.Village})"
            }).Select(x => new TotalWithMonths
            {
               // Month = x.Key.Month,
                RuralGovName = x.Key.Name,
                Year = x.Key.year,
                January=x.Where(c=>c.Month==Shared.Enums.Months.January).Sum(c=>c.Summa),
                February = x.Where(c => c.Month == Shared.Enums.Months.February).Sum(c => c.Summa),
                March = x.Where(c => c.Month == Shared.Enums.Months.March).Sum(c => c.Summa),
                April = x.Where(c => c.Month == Shared.Enums.Months.April).Sum(c => c.Summa),
                May = x.Where(c => c.Month == Shared.Enums.Months.May).Sum(c => c.Summa),
                June = x.Where(c => c.Month == Shared.Enums.Months.June).Sum(c => c.Summa),
                July = x.Where(c => c.Month == Shared.Enums.Months.July).Sum(c => c.Summa),
                August = x.Where(c => c.Month == Shared.Enums.Months.August).Sum(c => c.Summa),
                September= x.Where(c => c.Month == Shared.Enums.Months.September).Sum(c => c.Summa),
                October= x.Where(c => c.Month == Shared.Enums.Months.October).Sum(c => c.Summa),
                November= x.Where(c => c.Month == Shared.Enums.Months.November).Sum(c => c.Summa),
                December= x.Where(c => c.Month == Shared.Enums.Months.December).Sum(c => c.Summa),

            });
        return query;
    }
    //public async Task<PaginationResponse<TotalByRuralGovDto>> Handle(GetStateByRuralGovRequest request, CancellationToken cancellationToken)
    //{
    //    var expressein =  dapperRepository.GetQueryable<Contribution>()
    //        .WithSpecification(new GetStateByRuralGovRequestSpec(request));
       
    //    var query=await getSummAllMonth(expressein)
    //        .ToListAsync(cancellationToken);
    //    return new PaginationResponse<TotalByRuralGovDto>(query, query.Count(), request.PageNumber, request.PageSize);
    //}
    public async Task<PaginationResponse<TotalWithMonths>> Handle(GetStateByRuralGovRequest request, CancellationToken cancellationToken)
    {
        var expressein = dapperRepository.GetQueryable<Contribution>()
            .WithSpecification(new GetStateByRuralGovRequestSpec(request));

        var query = await getSummColMonths(expressein)
            .ToListAsync(cancellationToken);
        var total = new TotalWithMonths
        {
            Year=0,
            RuralGovName= stringLocalizer.GetString("Total"),
            January=query.Sum(x=>x.January),
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
            Style= "font-weight: bold;"
        };
        query.Add(total);
        return new PaginationResponse<TotalWithMonths>(query, query.Count(), request.PageNumber, request.PageSize);
    }
}