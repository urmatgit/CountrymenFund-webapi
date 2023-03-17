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
public class ExportTotalByNativesRequest : BaseFilter, IRequest<Stream>
{
    public DefaultIdType? YearId { get; set; }
    public DefaultIdType? RuralGovId { get; set; }
}
public class ExportTotalReportByNativesRequestSpec : EntitiesByBaseFilterSpec<Contribution, TotalByNative>
{
    public ExportTotalReportByNativesRequestSpec(ExportTotalByNativesRequest request) : base(request)
    => Query
        .Include(p => p.Year)
        .Include(p => p.Native)
        .ThenInclude(p => p.RuralGov)
        //.OrderByDescending(o => o.Year.year, !request.HasOrderBy())
        //.ThenBy(o => o.Native.RuralGov.Name, !request.HasOrderBy())
        .Where(p => p.YearId == request.YearId, request.YearId.HasValue)
        .Where(p => p.Native.RuralGovId == request.RuralGovId, request.RuralGovId.HasValue)
        ;
}


public class ExportTotalByNativesRequestHandler : IRequestHandler<ExportTotalByNativesRequest, Stream>
{
    private readonly IDapperRepository _dapperRepository;
    private readonly IExcelWriter _excelWriter;
    private readonly IStringLocalizer<ExportTotalByNativesRequestHandler> _stringLocalizer;
    public ExportTotalByNativesRequestHandler(IDapperRepository dapperRepository,IExcelWriter excelWriter, IStringLocalizer<ExportTotalByNativesRequestHandler> stringLocalizer)
    {
        _dapperRepository = dapperRepository;
        _excelWriter = excelWriter;
        this._stringLocalizer = stringLocalizer;
    }

    public async Task<Stream> Handle(ExportTotalByNativesRequest request, CancellationToken cancellationToken)
    {
        
        var totaByNative = new GetTotalByNativeData(_dapperRepository, _stringLocalizer);
        var list = await totaByNative.GetListByNatives(new ExportTotalReportByNativesRequestSpec(request), false, cancellationToken);

        //var list = await _repository.ListAsync(spec, cancellationToken);

        return _excelWriter.WriteToStream(list);
    }
}