using FSH.WebApi.Application.Catalog.FSContributions;
using FSH.WebApi.Application.Catalog.Natives;
using FSH.WebApi.Application.Common.Exporters;
using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.FSContributions;
public class ExportFSContributionsRequest: BaseFilter,IRequest<byte[]>
{

    
    public DefaultIdType? NativeId { get; set; }
    public DefaultIdType? FinSupportId { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }

    public string? FIO { get; set; }
}
public class ExportFSContributionsRequestHandler : IRequestHandler<ExportFSContributionsRequest, byte[]>
{
    private readonly IReadRepository<FSContribution> _repository;
    private readonly IExcelWriter _excelWriter;
    private readonly IStringLocalizer<FSContributionExportDto> _localizer;
    public ExportFSContributionsRequestHandler(IReadRepository<FSContribution> repository, IExcelWriter excelWriter, IStringLocalizer<FSContributionExportDto> localizer)
    {
        _repository = repository;
        _excelWriter = excelWriter;
        _localizer = localizer;
    }

    public async Task<byte[]> Handle(ExportFSContributionsRequest request, CancellationToken cancellationToken)
    {
        var spec = new ExportFSContributionsWithBrandsSpecification(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        
        var result = await _excelWriter.ExportAsync(list,
            new Dictionary<string, Func<FSContributionExportDto, object>>()
            {
                    { _localizer["NativeFIO"], item => item.NativeFIO },
                    { _localizer["FinSuportName"], item => item.FinSuportName },
                    { _localizer["Summa"], item => item.Summa },
                    { _localizer["Date"], item => item.Date },
                    { _localizer["Description"], item => item.Description }

            }, _localizer["FSContributions"]
            );

        return result;
        //return _excelWriter.WriteToStream(list);
    }
}

public class ExportFSContributionsWithBrandsSpecification : EntitiesByBaseFilterSpec<FSContribution, FSContributionExportDto>
{
    public ExportFSContributionsWithBrandsSpecification(ExportFSContributionsRequest request)
        : base(request) =>
        Query
       .Include(p => p.FinSupport)
       .Include(p => p.Native)
       .OrderByDescending(p => p.FinSupport.Begin)
       .ThenBy(p => p.Date)
       .ThenBy(p => p.Native.Name)
        .Where(p=>p.FinSupportId.Equals(request.FinSupportId),request.FinSupportId.HasValue)
       .Where(p => p.NativeId.Equals(request.NativeId), request.NativeId.HasValue)
       .Where(p => p.Date >= request.DateStart!.Value.ToUniversalTime() && p.Date <= request.DateEnd!.Value.ToUniversalTime(), request.DateStart.HasValue && request.DateEnd.HasValue)
       .Where(p => p.Native.Name.Contains(request.FIO) || p.Native.Surname.Contains(request.FIO)
       || (!string.IsNullOrEmpty(p.Native.MiddleName) && p.Native.MiddleName.Contains(request.FIO))
       || p.FinSupport.Name.Contains(request.FIO)
       || p.Summa.ToString().Contains(request.FIO)
       || (p.Description!.Contains(request.FIO) && !string.IsNullOrEmpty(p.Description)), !string.IsNullOrEmpty(request.FIO));

}