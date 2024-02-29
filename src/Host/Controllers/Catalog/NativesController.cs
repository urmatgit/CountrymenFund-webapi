using FSH.WebApi.Application.Catalog.Brands;
using FSH.WebApi.Application.Catalog.Natives;
using FSH.WebApi.Application.Catalog.Natives;

namespace FSH.WebApi.Host.Controllers.Catalog;

public class NativesController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search,FSHResource.Natives)]
    [OpenApiOperation("Search natives using available filters.","")]
    public Task<PaginationResponse<NativeDto>> SearchAsync(SearchNativesRequest request)
    {
        return Mediator.Send(request);
    }
    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Natives)]
    [OpenApiOperation("Get native details.", "")]
    public Task<NativeDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetNativeRequest(id));
    }

    //[HttpGet("dapper")]
    //[MustHavePermission(FSHAction.View, FSHResource.Natives)]
    //[OpenApiOperation("Get native details via dapper.", "")]
    //public Task<NativeDto> GetDapperAsync(Guid id)
    //{
    //    return Mediator.Send(new GetNativeViaDapperRequest(id));
    //}

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Natives)]
    [OpenApiOperation("Create a new native.", "")]
    public Task<Guid> CreateAsync(CreateNativeRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Natives)]
    [OpenApiOperation("Update a native.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateNativeRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Natives)]
    [OpenApiOperation("Delete a native.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteNativeRequest(id));
    }

    [HttpPost("export")]
    [MustHavePermission(FSHAction.Export, FSHResource.Natives)]
    [OpenApiOperation("Export a natives.", "")]
    public async Task<FileResult> ExportAsync(ExportNativesRequest filter)
    {
        var result = await Mediator.Send(filter);
        return File(result, "application/octet-stream", "NativeExports");
    }

    [HttpGet("generate/{nseed:int}")]
    [MustHavePermission(FSHAction.Generate, FSHResource.Natives)]
    [OpenApiOperation("Generate a number of random brands.", "")]
    public Task<string> GenerateRandomAsync(int nseed)
    {
        var request = new GenereateRandomNativeRequest()
        {
            NSeed = nseed
        };
        return Mediator.Send(request);
    }
}
