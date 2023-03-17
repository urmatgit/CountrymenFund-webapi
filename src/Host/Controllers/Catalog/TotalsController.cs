using FSH.WebApi.Application.Catalog.Natives;
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
    [HttpPost("exportbynative")]
    [MustHavePermission(FSHAction.Export, FSHResource.Reports)]
    [OpenApiOperation("Export total by natives.", "")]
    public async Task<FileResult> ExportAsync(ExportTotalByNativesRequest filter)
    {
        var result = await Mediator.Send(filter);
        return File(result, "application/octet-stream", "TotalByNativeExports");
    }
    // ExportTotalByRuralGovsRequest
    [HttpPost("exportbyruralgov")]
    [MustHavePermission(FSHAction.Export, FSHResource.Reports)]
    [OpenApiOperation("Export total by ruralgov.", "")]
    public async Task<FileResult> ExportByRuralGovsAsync(ExportTotalByRuralGovsRequest filter)
    {
        var result = await Mediator.Send(filter);
        return File(result, "application/octet-stream", "TotalByRuralGovExports");
    }
}
