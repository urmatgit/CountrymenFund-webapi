using FSH.WebApi.Application.Catalog.Contributions;
using FSH.WebApi.Application.Identity.Roles;
using FSH.WebApi.Application.Identity.Users;
using FSH.WebApi.Domain.Catalog.Fund;
using System.Threading;

namespace FSH.WebApi.Application.Dashboard;

public class GetStatsRequest : IRequest<StatsDto>
{
}

public class GetStatsRequestHandler : IRequestHandler<GetStatsRequest, StatsDto>
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly IReadRepository<Brand> _brandRepo;
    private readonly IReadRepository<RuralGov> _ruralGovRepo;
    private readonly IReadRepository<Product> _productRepo;
    private readonly IReadRepository<Native> _nativeRepo;
    private readonly IReadRepository<Contribution> _contributionRepo;
    private readonly IDapperRepository _dapperRepository;
    private readonly IStringLocalizer _t;
    private readonly GetContributionsSummRequest _sumRequest;
    public GetStatsRequestHandler(IUserService userService, IRoleService roleService, IReadRepository<Brand> brandRepo, IReadRepository<RuralGov> ruralGovRepo, IReadRepository<Product> productRepo, IStringLocalizer<GetStatsRequestHandler> localizer, IReadRepository<Native> nativeRepo, IReadRepository<Contribution> contributionRepo, IDapperRepository dapperRepository)
    {
        _userService = userService;
        _roleService = roleService;
        _brandRepo = brandRepo;
        _ruralGovRepo = ruralGovRepo;
        _productRepo = productRepo;
        _t = localizer;
        _nativeRepo = nativeRepo;
        _contributionRepo = contributionRepo;
        _dapperRepository = dapperRepository;
        _sumRequest = new GetContributionsSummRequest(_dapperRepository, _contributionRepo);
    }
    private async Task<decimal> GetContributesSum(CancellationToken cancellationToken)
    {
        //   var request= new GetContributionsSummRequest(_dapperRepository,_contributionRepo);
        return await _sumRequest.GetSumm(cancellationToken);
    }
    private async Task<decimal> GetContributesSumBitween(DateTime start, DateTime until, CancellationToken cancellationToken)
    {
        // var request = new GetContributionsSummRequest(_dapperRepository, _contributionRepo);

        return await _sumRequest.GetSummBitween(start, until, cancellationToken);
    }
    private async Task<IEnumerable<GroupTotal>> GetGroupTotals(DateTime start, DateTime until, CancellationToken cancellationToken)
    {
        return await _sumRequest.GetRuralGovsSummBitween(start, until, cancellationToken);
    }
    public async Task<StatsDto> Handle(GetStatsRequest request, CancellationToken cancellationToken)
    {
        var stats = new StatsDto
        {
            ProductCount = await _productRepo.CountAsync(cancellationToken),
            BrandCount = await _brandRepo.CountAsync(cancellationToken),
            RuralGovCount = await _ruralGovRepo.CountAsync(cancellationToken),
            UserCount = await _userService.GetCountAsync(cancellationToken),
            RoleCount = await _roleService.GetCountAsync(cancellationToken),
            NativeCount = await _nativeRepo.CountAsync(cancellationToken),
            ContributionsCount = await _contributionRepo.CountAsync(cancellationToken),
            ContributionSumma = await GetContributesSum(cancellationToken)

        };

        int selectedYear = DateTime.UtcNow.Year;
        double[] productsFigure = new double[13];
        double[] brandsFigure = new double[13];
        double[] ruralGovsFigure = new double[13];
        double[] nativeFigure = new double[13];
        double[] contributionFigure = new double[13];
        double[] contributionSumFigure = new double[13];
        Dictionary<string, double[]> ruralGovsTotals = new Dictionary<string, double[] > ();
        for (int i = 1; i <= 12; i++)
        {
            int month = i;
            var filterStartDate = new DateTime(selectedYear, month, 01).ToUniversalTime();
            var filterEndDate = new DateTime(selectedYear, month, DateTime.DaysInMonth(selectedYear, month), 23, 59, 59).ToUniversalTime(); // Monthly Based

            var brandSpec = new AuditableEntitiesByCreatedOnBetweenSpec<Brand>(filterStartDate, filterEndDate);
            var ruralGovSpec = new AuditableEntitiesByCreatedOnBetweenSpec<RuralGov>(filterStartDate, filterEndDate);
            var productSpec = new AuditableEntitiesByCreatedOnBetweenSpec<Product>(filterStartDate, filterEndDate);
            var nativeSpec = new AuditableEntitiesByCreatedOnBetweenSpec<Native>(filterStartDate, filterEndDate);
            var contributionSpec = new AuditableEntitiesByCreatedOnBetweenSpec<Contribution>(filterStartDate, filterEndDate);

            brandsFigure[i - 1] = await _brandRepo.CountAsync(brandSpec, cancellationToken);
            ruralGovsFigure[i - 1] = await _ruralGovRepo.CountAsync(ruralGovSpec, cancellationToken);
            productsFigure[i - 1] = await _productRepo.CountAsync(productSpec, cancellationToken);
            nativeFigure[i - 1] = await _nativeRepo.CountAsync(nativeSpec, cancellationToken);
            contributionFigure[i - 1] = await _contributionRepo.CountAsync(contributionSpec, cancellationToken);
            contributionSumFigure[i - 1] = (double)await GetContributesSumBitween(filterStartDate, filterEndDate, cancellationToken);
            foreach (var gt in await GetGroupTotals(filterStartDate, filterEndDate, cancellationToken))
                if (ruralGovsTotals.ContainsKey(gt.Name))
                {
                    ruralGovsTotals[gt.Name][i-1]=(double)gt.Total;
                }
                else
                {
                    var totalList = new double[13];
                    totalList[i-1]=(double)gt.Total;
                    ruralGovsTotals.Add(gt.Name, totalList);
                }
        }

        stats.DataEnterBarChart.Add(new ChartSeries { Name = _t["Products"], Data = productsFigure });
        stats.DataEnterBarChart.Add(new ChartSeries { Name = _t["Brands"], Data = brandsFigure });
        stats.DataEnterBarChart.Add(new ChartSeries { Name = _t["Rural goverments"], Data = ruralGovsFigure });
        stats.DataEnterBarChart.Add(new ChartSeries { Name = _t["Natives"], Data = nativeFigure });
        stats.DataEnterBarChart.Add(new ChartSeries { Name = _t["Contributions"], Data = contributionFigure });
        stats.DataEnterSumBarChart.Add(new ChartSeries { Name = _t["Contribution summa"], Data = contributionSumFigure });
        foreach (var key in ruralGovsTotals)
        {
            stats.DataEnterSumBarChart.Add(new ChartSeries
            {
                Name = key.Key,
                Data = key.Value
            });
        }
        return stats;
    }
}
