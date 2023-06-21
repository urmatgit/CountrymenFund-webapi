using FSH.WebApi.Application.Catalog.Natives;
using FSH.WebApi.Application.Catalog.RuralGovs;
using FSH.WebApi.Application.Catalog.Years;
using FSH.WebApi.Application.Common.DataIO;
using FSH.WebApi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class ImportContributionRequest: ImportRequest<int>
{
    public DefaultIdType RuralGovId { get; set; }
    
}
public class ImportContributionRequestHandler : IRequestHandler<ImportContributionRequest, int>
{

    private readonly IRepositoryWithEvents<Contribution> _repository;
    private readonly ILogger<ImportContributionRequestHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IStringLocalizer _stringLocalizer;
    private readonly IExcelReader _excelReader;
    private readonly IRepositoryWithEvents<Native> _nativeRepo;
    private readonly IReadRepository<Year> _yearRepo;
    public ImportContributionRequestHandler(IRepositoryWithEvents<Native> nativeRepo, IRepositoryWithEvents<Contribution> repository, ILogger<ImportContributionRequestHandler> logger, IStringLocalizer<ImportContributionRequestHandler> stringLocalizer, IExcelReader excelReader, IReadRepository<Year> yearRepo,
        IMediator mediator)
    {
        _mediator = mediator;
        _repository = repository;
        _logger = logger;
        _nativeRepo = nativeRepo;
        _stringLocalizer = stringLocalizer;
        _excelReader = excelReader;
        _yearRepo = yearRepo;
    }
    private async Task<YearDto> GetYearId(int? year)
    {
        return await _mediator.Send(new GetYearByYearRequest(year ?? DateTime.Now.Year));
    }
    private Months GetMonthByName (string name)
    {
        return (Months)Enum.Parse(typeof(Months), name);
    }
    private async Task<Contribution> CreateContribution(Guid nativeId,int? year, Months month, decimal summa)
    {
        DateTime addDate = DateTime.Parse($"25 {month} {year ?? DateTime.Now.Year}");
        var yearID = await GetYearId(year);
        return new Contribution(summa, month, addDate, nativeId, yearID.Id, "Import From Excel");
    }
    private decimal? GetPropertyValue(ImportContributionDto obj, string name)
    {
        return obj.GetType().GetProperty(name).GetValue(obj, null) as decimal?;
    }
    public async Task<int> Handle(ImportContributionRequest request, CancellationToken cancellationToken)
    {
        var items = await _excelReader.ToListAsync<ImportContributionDto>(request.ExcelFile, FileType.Excel, request.SheetName,
            new Dictionary<string, string>()
            {
                { "Name","Аты" },
                { "Surname","Фамилия|Теги:" },
                { "Village","Айыл|Кенеш:" },
                {"January","Январь|Янв." },
                {"February","Январь|Янв." },
                {"March","Март" },
                {"April","Апрель|Апр." },
                {"May","Май" },
                {"June","Июнь" },
                {"July","Июль" },
                {"August","Август|Авг." },
                {"September","Сентябрь|Сен." },
                {"October","Октябрь|Окт." },
                {"November","Ноябрь|Ноя." },
                {"December","Декабрь|Дек." },


                });

        if (items?.Count > 0)
        {
            try
            {
                foreach (var item in items)
                {
                    if (string.IsNullOrEmpty(item.Name)) continue;
                    var existsNative = await _nativeRepo.GetBySpecAsync(new NativeCheckExistSpec(item.Name, item.Surname, ruralId: request.RuralGovId, birchDate: null, hasImage: false));
                    if (existsNative is null)
                    {
                         existsNative = new Native(item.Name, item.Surname, village: item.Village, ruralGovId: request.RuralGovId,middlename: "",birthdate: null,rate: 5,description: null,imagepath: "");
                        
                        await _nativeRepo.AddAsync(existsNative, cancellationToken);
                        await _nativeRepo.SaveChangesAsync(cancellationToken);
                    }
                    // January
                    for(int indexMonth = 1; indexMonth <= 12; indexMonth++)
                    {
                        decimal summ = GetPropertyValue(item, ((Months)indexMonth).ToString())?? 0m;
                        if (summ == 0) continue;
                        var contr = await CreateContribution(existsNative.Id, DateTime.Now.Year, (Months)indexMonth,summ);
                        var finded = await _repository.GetBySpecAsync(new FindContributionByParamSpec(contr), cancellationToken);
                        if (finded is null)
                        {
                            await _repository.AddAsync(contr, cancellationToken);
                        }
                    }
                    

                }
                await _repository.SaveChangesAsync(cancellationToken);
                return items.Count;
            }
            catch (Exception er )
            {
                _logger.LogError(er,"Import Contribution error");
                throw new InternalServerException(_stringLocalizer["Internal error!"]);
            }
        }

        return 0;
    }
}
