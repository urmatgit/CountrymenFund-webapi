using FSH.WebApi.Application.Common.Exporters;
using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class ExportNativesRequest : BaseFilter,IRequest<Stream>
{
    public DefaultIdType? RuralGovId { get; set; }
}
public class ExportNativesRequestHandler: IRequestHandler<ExportNativesRequest, Stream>
{
    private readonly IReadRepository<Native> _repository;
    private readonly IExcelWriter _excelWriter;
    public ExportNativesRequestHandler(IReadRepository<Native> repository, IExcelWriter excelWriter)
    {
        _repository = repository;
        _excelWriter = excelWriter; 
    }

    public async Task<Stream> Handle(ExportNativesRequest request, CancellationToken cancellationToken)
    {
        var spec = new ExportNativesWithRuralGovSpecification(request);
         var list = await _repository.ListAsync(spec,cancellationToken);
        return _excelWriter.WriteToStream(list);
    }
}
public class ExportNativesWithRuralGovSpecification: EntitiesByBaseFilterSpec<Native, NativeExportDto>
{
    public ExportNativesWithRuralGovSpecification( ExportNativesRequest request): base(request)
    {
        Query.Include(p => p.RuralGov)
            .Where(p => p.RuralGovId.Equals(request.RuralGovId!.Value), request.RuralGovId.HasValue);
    }
}
