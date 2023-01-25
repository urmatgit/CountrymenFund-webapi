using FSH.WebApi.Application.Catalog.Years;

namespace FSH.WebApi.Host.Controllers.Catalog;

public class YearsController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.Years)]
    [OpenApiOperation("Search years using available filters.", "")]
    public Task<PaginationResponse<YearDto>> SearchAsync(SearchYearsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.Years)]
    [OpenApiOperation("Get year details.", "")]
    public Task<YearDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetYearRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.Years)]
    [OpenApiOperation("Create a new year.", "")]
    public Task<Guid> CreateAsync(CreateYearRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.Years)]
    [OpenApiOperation("Update a year.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateYearRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.Years)]
    [OpenApiOperation("Delete a year.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteYearRequest(id));
    }

    
}
