
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Domain.Catalog.Fund;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FSH.WebApi.Application.Catalog.Totals;
public class GetTotalByNativeData
{
    private readonly IDapperRepository _dapperRepository;
    private readonly IStringLocalizer stringLocalizer;
    public GetTotalByNativeData(IDapperRepository dapperRepository, IStringLocalizer stringLocalizer)
    {
        _dapperRepository = dapperRepository;
        this.stringLocalizer = stringLocalizer;
    }
    public async Task<List<TotalWithMonths>> GetListByRuralGovs(ISpecification<Contribution, TotalWithMonths> specification, bool hasOrder, CancellationToken cancellationToken)
    {
        var expressein = _dapperRepository.GetQueryable<Contribution>()
           .WithSpecification(specification);

        var query = await getSummColMonthsForRuralGov(expressein)
            .ToListAsync(cancellationToken);
        var total = new TotalWithMonths
        {
            Year = 0,
            RuralGovName = stringLocalizer.GetString("Total"),
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
        return query;
    }
    private IQueryable<TotalWithMonths> getSummColMonthsForRuralGov(IQueryable<Contribution> queryable)
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

    public async Task<List<TotalByNative>> GetListByNatives( ISpecification<Contribution, TotalByNative> specification,bool hasOrder, CancellationToken cancellationToken)
    {
        var expressein = _dapperRepository.GetQueryable<Contribution>()
            .WithSpecification(specification);

        var query = await getSummColMonths(expressein)
                       .ToListAsync(cancellationToken);
        if (!hasOrder && query != null)
        {
            query = query.OrderBy(o => o.Year)
            .ThenBy(o => o.RuralGovName)
            .ThenBy(o => o.FIO)
            .ToList();
        }
        var total = new TotalByNative
        {
            Year = 0,
            FIO = stringLocalizer.GetString("Totals"),
            RuralGovName = stringLocalizer.GetString("Totals"),
            Village = "",

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
        return query;
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
