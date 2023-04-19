using FSH.WebApi.Application.Catalog.Contributions;
using FSH.WebApi.Application.Catalog.Natives;
using FSH.WebApi.Application.Common.Exporters;
using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class ExportContributionsRequest: BaseFilter,IRequest<Stream>
{

    public DefaultIdType? YearId { get; set; }
    public DefaultIdType? NativeId { get; set; }
    public DefaultIdType? RuralGovId { get; set; }
    public Months? Month { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }

    public string? FIO { get; set; }
}
public class ExportContributionsRequestHandler : IRequestHandler<ExportContributionsRequest, Stream>
{
    private readonly IReadRepository<Contribution> _repository;
    private readonly IExcelWriter _excelWriter;
    private readonly IStringLocalizer<ContributionExportDto> _localizer;
    public ExportContributionsRequestHandler(IReadRepository<Contribution> repository, IExcelWriter excelWriter,IStringLocalizer<ContributionExportDto> stringLocalizer    )
    {
        _repository = repository;
        _excelWriter = excelWriter;
        _localizer = stringLocalizer;
    }

    public async Task<Stream> Handle(ExportContributionsRequest request, CancellationToken cancellationToken)
    {
        var spec = new ExportContributionsWithBrandsSpecification(request);

        var list = await _repository.ListAsync(spec, cancellationToken);
        var result = await _excelWriter.ExportExcelAsync(list,
           new Dictionary<string, Func<ContributionExportDto, object>>()
           {
                    { _localizer["Year"], item => item.Year },
                    { _localizer["RuralGovName"], item => item.RuralGovName },
                    { _localizer["NativeFIO"], item => item.NativeFIO },
                    { _localizer["Month"], item => item.Month },
                    { _localizer["Summa"], item => item.Summa },
                    { _localizer["Date"], item => item.Date },
                    { _localizer["Village"], item => item.Village },
                    { _localizer["Description"], item => item.Description },

           }, _localizer["Natives"]
           );

        return result;
        //return _excelWriter.WriteToStream(list);
    }
}

public class ExportContributionsWithBrandsSpecification : EntitiesByBaseFilterSpec<Contribution, ContributionExportDto>
{
    public ExportContributionsWithBrandsSpecification(ExportContributionsRequest request)
        : base(request) =>
        Query
       .Include(p => p.Year)
       .Include(p => p.Native)
       .ThenInclude(p => p.RuralGov)
       .OrderByDescending(p => p.Year.year)
       .ThenBy(p => p.Month)
       .ThenBy(p => p.Native.RuralGov.Name)
       .ThenBy(p => p.Native.Name)
       .Where(p => p.YearId.Equals(request.YearId), request.YearId.HasValue)
       .Where(p => p.NativeId.Equals(request.NativeId), request.NativeId.HasValue)
       .Where(p => p.Native.RuralGovId.Equals(request.RuralGovId), request.RuralGovId.HasValue)
       .Where(p => p.Month == request.Month, request.Month.HasValue)
       .Where(p => p.Date >= request.DateStart!.Value.ToUniversalTime() && p.Date <= request.DateEnd!.Value.ToUniversalTime(), request.DateStart.HasValue && request.DateEnd.HasValue)
       .Where(p => p.Native.Name.Contains(request.FIO) || p.Native.Surname.Contains(request.FIO)
       || (!string.IsNullOrEmpty(p.Native.MiddleName) && p.Native.MiddleName.Contains(request.FIO))
       || p.Native.RuralGov.Name.Contains(request.FIO)
       || p.Summa.ToString().Contains(request.FIO)
       || (p.Description!.Contains(request.FIO) && !string.IsNullOrEmpty(p.Description)), !string.IsNullOrEmpty(request.FIO));

}