using FSH.WebApi.Application.Catalog.Products;
using FSH.WebApi.Application.Common.Exporters;
using FSH.WebApi.Domain.Catalog.Fund;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FSH.WebApi.Application.Catalog.Natives;
public class ExportNativesRequest : BaseFilter,IRequest<byte[]>
{
    public DefaultIdType? RuralGovId { get; set; }
    
}
public class ExportNativesRequestHandler: IRequestHandler<ExportNativesRequest, byte[]>
{
    private readonly IReadRepository<Native> _repository;
    private readonly IExcelWriter _excelWriter;
    private readonly IStringLocalizer<ExportNativesRequestHandler> _localizer;
    public ExportNativesRequestHandler(IReadRepository<Native> repository, IExcelWriter excelWriter, IStringLocalizer<ExportNativesRequestHandler> localizer)
    {
        _repository = repository;
        _excelWriter = excelWriter;
        _localizer = localizer;
    }

    public async Task<byte[]> Handle(ExportNativesRequest request, CancellationToken cancellationToken)
    {
    //     public string RuralGovName { get; set; }
    //public string Name { get; set; }
    //public string Surname { get; set; }
    //public string? MiddleName { get; set; }
    //public string Village { get; set; }
    //public DateTime? BirthDate { get; set; }

    //public string Description { get; set; }

    var spec = new ExportNativesWithRuralGovSpecification(request);
         var list = await _repository.ListAsync(spec,cancellationToken);
        var result = await _excelWriter.ExportAsync(list,
            new Dictionary<string, Func<NativeExportDto, object>>()
            {
                    { _localizer["RuralGovName"], item => item.RuralGovName },
                    { _localizer["Name"], item => item.Name },
                    { _localizer["Surname"], item => item.Surname },
                    { _localizer["MiddleName"], item => item.MiddleName },
                    { _localizer["Village"], item => item.Village },
                    { _localizer["BirthDate"], item => item.BirthDate },
                    { _localizer["Description"], item => item.Description },

            }, _localizer["Natives"]
            );
        
        return result;
        //return _excelWriter.WriteToStream(list);
    }
}
public class ExportNativesWithRuralGovSpecification: EntitiesByBaseFilterSpec<Native, NativeExportDto>
{
    public ExportNativesWithRuralGovSpecification( ExportNativesRequest request): base(request)
    {
        Query.Include(p => p.RuralGov)
            .Where(p => p.RuralGovId.Equals(request.RuralGovId!.Value), request.RuralGovId.HasValue)
            .OrderBy(o=>o.RuralGov.Name)
            .ThenBy(o=>o.Name)
            .ThenBy(o=>o.Surname);
            
             
    }
}
