using FSH.WebApi.Application.Catalog.Contributions;
using FSH.WebApi.Application.Catalog.Natives;
using FSH.WebApi.Application.Catalog.RuralGovs;
using MediatR;

namespace FSH.WebApi.Host.Controllers.Catalog;

public class ContributionsController: VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Contributions)]
    [OpenApiOperation("Search contributions using available filters.", "")]
    public Task<PaginationResponse<ContributionDto>> SearchAsync(SearchContributionsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Contributions)]
    [OpenApiOperation("Get contribution details.", "")]
    public Task<ContributionDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetContributionRequest(id));
    }
    [HttpGet("")]
    [MustHavePermission(FSHAction.View, FSHResource.Contributions)]
    [OpenApiOperation("Get contribution default.", "")]
    public Task<ContributionDto> GetDefaultAsync()
    {
        return Mediator.Send(new GetDefaultContributionRequest());
    }
    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Contributions)]
    [OpenApiOperation("Create a new contribution.", "")]
    public Task<Guid> CreateAsync(CreateContributionRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Contributions)]
    [OpenApiOperation("Update a contribution.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateContributionRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Contributions)]
    [OpenApiOperation("Delete a contribution.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteContributionRequest(id));
    }
    //ExportContributionRequest
    [HttpPost("export")]
    [MustHavePermission(FSHAction.Export, FSHResource.Contributions)]
    [OpenApiOperation("Export a contributions.", "")]
    public async Task<FileResult> ExportAsync(ExportContributionsRequest filter)
    {
        var result = await Mediator.Send(filter);
        return File(result, "application/octet-stream", "NativeExports");
    }

    [HttpPost("import")]
    [MustHavePermission(FSHAction.Import, FSHResource.Contributions)]
    [OpenApiOperation("Import a datas.", "")]
    public async Task<ActionResult<int>> ImportAsync(ImportContributionRequest request)
    {
        return Ok(await Mediator.Send(request));
    }

}
