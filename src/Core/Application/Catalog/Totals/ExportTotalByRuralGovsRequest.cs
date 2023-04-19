using FSH.WebApi.Application.Catalog.Contributions;
using FSH.WebApi.Application.Common.Exporters;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Domain.Catalog.Fund;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Totals;
public class ExportTotalByRuralGovsRequest : BaseFilter, IRequest<byte[]>
{
    public DefaultIdType? YearId { get; set; }
    
}
public class ExportTotalReportByRuralGovsRequestSpec : EntitiesByBaseFilterSpec<Contribution, TotalWithMonths>
{
    public ExportTotalReportByRuralGovsRequestSpec(ExportTotalByRuralGovsRequest request) : base(request)
    => Query
        .Include(p => p.Year)
        .Include(p => p.Native)
        .ThenInclude(p => p.RuralGov)
        .OrderByDescending(o => o.Year.year)
        .ThenBy(o => o.Native.RuralGov.Name)
        .Where(p => p.YearId == request.YearId, request.YearId.HasValue);
}


public class ExportTotalByRuralGovsRequestHandler : IRequestHandler<ExportTotalByRuralGovsRequest, byte[]>
{
    private readonly IDapperRepository _dapperRepository;
    private readonly IExcelWriter _excelWriter;
    private readonly IStringLocalizer<TotalWithMonths> _localizer;
    public ExportTotalByRuralGovsRequestHandler(IDapperRepository dapperRepository,IExcelWriter excelWriter, IStringLocalizer<TotalWithMonths> stringLocalizer)
    {
        _dapperRepository = dapperRepository;
        _excelWriter = excelWriter;
        this._localizer = stringLocalizer;
    }

    public async Task<byte[]> Handle(ExportTotalByRuralGovsRequest request, CancellationToken cancellationToken)
    {
        
        var totaByNative = new GetTotalByNativeData(_dapperRepository, _localizer);
        var list = await totaByNative.GetListByRuralGovs(new ExportTotalReportByRuralGovsRequestSpec(request), false, cancellationToken);

        var result = await _excelWriter.ExportAsync(list,
            new Dictionary<string, Func<TotalWithMonths, object>>()
            {
                    { _localizer["Year"], item => item.Year },
                    { _localizer["RuralGovName"], item => item.RuralGovName },
                    { _localizer["AllSumm"], item => item.AllSumm },
                    { _localizer["January"], item => item.January },
                    { _localizer["February"], item => item.February },
                    { _localizer["March"], item => item.March },
                    { _localizer["April"], item => item.April },
                    { _localizer["May"], item => item.May },
                    { _localizer["June"], item => item.June },
                    { _localizer["July"], item => item.July },
                    { _localizer["August"], item => item.August },
                    { _localizer["September"], item => item.September },
                    { _localizer["October"], item => item.October },
                    { _localizer["November"], item => item.November },
                    { _localizer["December"], item => item.December },

            }, _localizer["TotalByNative"]
            );

        return result;
        //return _excelWriter.WriteToStream(list);
    }
}