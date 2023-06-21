using FSH.WebApi.Application.Catalog.Contributions;
using FSH.WebApi.Application.Common.DataIO;
using FSH.WebApi.Application.Common.Exporters;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FSH.WebApi.Application.Catalog.RuralGovs;
public class ImportRuralGovReques: ImportRequest<int>
{
    
}

public class ImportRuralGovRequesHandler :
    
    IRequestHandler<ImportRuralGovReques, int>
{
    private readonly IRepositoryWithEvents<RuralGov> _repository;
    private readonly ILogger<ImportRuralGovRequesHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IStringLocalizer _stringLocalizer;
    private readonly IExcelReader _excelReader;
    public ImportRuralGovRequesHandler(IMediator mediator, IRepositoryWithEvents<RuralGov> repository, ILogger<ImportRuralGovRequesHandler> logger, IStringLocalizer<ImportRuralGovRequesHandler> stringLocalizer,IExcelReader excelReader)
    {

        _repository = repository;
        _logger = logger;
        _mediator= mediator;
        _stringLocalizer = stringLocalizer;
        _excelReader = excelReader;
    }
    public async Task<int> Handle(ImportRuralGovReques request, CancellationToken cancellationToken)
    {
        var items = await _excelReader.ToListAsync<RuralGov>(request.ExcelFile, FileType.Excel, "ИтогоКараКулжаРайону",
            new Dictionary<string, string>()
            {
                { "Name","Айыл аймактары:" }
                });

        if (items?.Count > 0)
        {
            try
            {
                
                foreach (var item in items)
                {

                    if (string.IsNullOrEmpty(item.Name)) continue;
                    
                    var exists = await _repository.GetBySpecAsync(new RuralGovByNameSpec(item.Name), cancellationToken);
                    if (exists is null)
                    {
                        exists = await _repository.AddAsync(new RuralGov(item.Name, ""), cancellationToken);
                        
                    }
                    


                   var result=  await _mediator.Send(new ImportContributionRequest()
                    {
                        ExcelFile = request.ExcelFile,
                        RuralGovId = exists.Id,
                        SheetName = exists.Name
                    });
                }
              return  items.Count;
            }
            catch (Exception er)
            {
                throw new InternalServerException(_stringLocalizer["Internal error!"]);
            }
        }

        return 0;
    }
    //public async Task<Result<List<string>>> Handle1(ImportRuralGovReques request, CancellationToken cancellationToken)
    //{

    //    try
    //    {
    //        string base64Data = Regex.Match(request.ExcelFile.Data, string.Format("data:{0}/(?<type>.+?),(?<data>.+)", FileType.Excel.ToString().ToLower())).Groups["data"].Value;
    //        var streamData = new MemoryStream(Convert.FromBase64String(base64Data));
    //        var data = await _excelWriter.ImportAsync(streamData.ToArray(), sheetName: "ИтогоКараКулжаРайону", mappers: new Dictionary<string, Func<System.Data.DataRow, RuralGovDto, object>>
    //    {
    //        {"Айыл аймактары:",(row,item)=>item.Name=row["Айыл аймактары:"]?.ToString() }
    //    });
    //        if (data.Succeeded && data.Data is not null)
    //        {
    //            foreach (var item in data.Data)
    //            {
    //                var exists = await _repository.AnyAsync(new RuralGovByNameSpec(item.Name), cancellationToken);
    //                if (!exists)
    //                {
    //                    await _repository.AddAsync(new RuralGov(item.Name, ""), cancellationToken);

    //                }
    //            }
    //            await _repository.SaveChangesAsync(cancellationToken);
    //            return Result<List<string>>.Success(data.Data.Select(d => d.Name).ToList());
    //        }
    //        return Result<List<string>>.Success(new List<string>());
    //    }catch(Exception er)
    //    {
    //        throw new InternalServerException(_stringLocalizer["Internal error:"], new List<string>() { er.Message,er.InnerException?.Message });
    //    }
    //}

     
}
