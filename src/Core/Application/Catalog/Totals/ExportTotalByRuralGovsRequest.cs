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
public class ExportTotalByRuralGovsRequest : BaseFilter, IRequest<Stream>
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


public class ExportTotalByRuralGovsRequestHandler : IRequestHandler<ExportTotalByRuralGovsRequest, Stream>
{
    private readonly IDapperRepository _dapperRepository;
    private readonly IExcelWriter _excelWriter;
    private readonly IStringLocalizer<ExportTotalByRuralGovsRequestHandler> _stringLocalizer;
    public ExportTotalByRuralGovsRequestHandler(IDapperRepository dapperRepository,IExcelWriter excelWriter, IStringLocalizer<ExportTotalByRuralGovsRequestHandler> stringLocalizer)
    {
        _dapperRepository = dapperRepository;
        _excelWriter = excelWriter;
        this._stringLocalizer = stringLocalizer;
    }

    public async Task<Stream> Handle(ExportTotalByRuralGovsRequest request, CancellationToken cancellationToken)
    {
        
        var totaByNative = new GetTotalByNativeData(_dapperRepository, _stringLocalizer);
        var list = await totaByNative.GetListByRuralGovs(new ExportTotalReportByRuralGovsRequestSpec(request), false, cancellationToken);

        //var list = await _repository.ListAsync(spec, cancellationToken);

        return _excelWriter.WriteToStream(list);
    }
}