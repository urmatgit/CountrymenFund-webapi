using FSH.WebApi.Application.Catalog.Totals;

namespace FSH.WebApi.Host.Controllers.Catalog;

public class TotalsController  : VersionedApiController
{
    [HttpPost]
    [MustHavePermission(FSHAction.View, FSHResource.Reports)]
    //[AllowAnonymous]
    [OpenApiOperation("Get statistics for the dashboard.", "")]
    public async Task<PaginationResponse<TotalWithMonths>> GetTotalRuralgovAsync(GetStateByRuralGovRequest request)
    {
        return await Mediator.Send(request);
    }
    [HttpPost("ByNative")]
    [MustHavePermission(FSHAction.View,FSHResource.Reports)]
    [OpenApiOperation("Get statistics for the dashboard.", "")]
    public async Task<PaginationResponse<TotalByNative>> GetTotalByNativeAsync(GetTotalReportByNativesRequest request)
    {
        return await Mediator.Send(request);
    }
}
