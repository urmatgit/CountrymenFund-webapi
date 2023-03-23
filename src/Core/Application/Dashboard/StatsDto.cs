namespace FSH.WebApi.Application.Dashboard;

public class StatsDto
{
    //public int ProductCount { get; set; }
    //public int BrandCount { get; set; }
    public int RuralGovCount { get; set; }
    public int UserCount { get; set; }
    public int RoleCount { get; set; }
    public int NativeCount { get; set; }
    public int ContributionsCount { get; set; }
    public decimal ContributionSumma { get; set; }
    public decimal FSContributionSumma { get; set; }
    public List<ChartSeries> DataEnterBarChart { get; set; } = new();
    public List<ChartSeries> DataEnterSumBarChart { get; set; } = new();
    public Dictionary<string, double>? ProductByBrandTypePieChart { get; set; }
}

public class ChartSeries
{
    public string? Name { get; set; }
    public double[]? Data { get; set; }
}