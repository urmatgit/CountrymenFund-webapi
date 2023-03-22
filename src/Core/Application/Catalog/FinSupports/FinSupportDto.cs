namespace FSH.WebApi.Application.Catalog.FinSupports;

public class FinSupportDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }
    public DefaultIdType NativeId { get; set; }
    public string FIOManager { get; set; }
}