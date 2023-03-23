using FSH.WebApi.Application.Catalog.FSContributions;
using FSH.WebApi.Application.Catalog.Natives;
using MediatR;

namespace FSH.WebApi.Host.Controllers.Catalog;

public class FSContributionsController: VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.FSContributions)]
    [OpenApiOperation("Search FSContributions using available filters.", "")]
    public Task<PaginationResponse<FSContributionDto>> SearchAsync(SearchFSContributionsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.FSContributions)]
    [OpenApiOperation("Get FSContribution details.", "")]
    public Task<FSContributionDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetFSContributionRequest(id));
    }
    [HttpGet("")]
    [MustHavePermission(FSHAction.View, FSHResource.FSContributions)]
    [OpenApiOperation("Get FSContribution default.", "")]
    public Task<FSContributionDto> GetDefaultAsync()
    {
        return Mediator.Send(new GetDefaultFSContributionRequest());
    }
    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.FSContributions)]
    [OpenApiOperation("Create a new FSContribution.", "")]
    public Task<Guid> CreateAsync(CreateFSContributionRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.FSContributions)]
    [OpenApiOperation("Update a FSContribution.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateFSContributionRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.FSContributions)]
    [OpenApiOperation("Delete a FSContribution.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteFSContributionRequest(id));
    }
    //ExportFSContributionRequest
    [HttpPost("export")]
    [MustHavePermission(FSHAction.Export, FSHResource.FSContributions)]
    [OpenApiOperation("Export a FSContributions.", "")]
    public async Task<FileResult> ExportAsync(ExportFSContributionsRequest filter)
    {
        var result = await Mediator.Send(filter);
        return File(result, "application/octet-stream", "NativeExports");
    }

}
