using FSH.WebApi.Application.Catalog.Contributions;
using FSH.WebApi.Application.Common.DataIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public class NativeImportRequest: ImportRequest<int>
{
    public DefaultIdType GovRuralId { get; set; }
}
public class NativeImportRequestHandler : IRequestHandler<NativeImportRequest, int>
{
    private readonly IRepositoryWithEvents<Native> _repository;
    private readonly ILogger<NativeImportRequestHandler> _logger;

    private readonly IStringLocalizer _stringLocalizer;
    private readonly IExcelReader _excelReader;
    public NativeImportRequestHandler(IRepositoryWithEvents<Native> repository, ILogger<NativeImportRequestHandler> logger, IStringLocalizer<NativeImportRequestHandler> stringLocalizer, IExcelReader excelReader)
    {

        _repository = repository;
        _logger = logger;

        _stringLocalizer = stringLocalizer;
        _excelReader = excelReader;
    }
    public Task<int> Handle(NativeImportRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}