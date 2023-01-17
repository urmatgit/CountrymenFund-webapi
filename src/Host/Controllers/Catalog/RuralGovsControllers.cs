using FSH.WebApi.Application.Catalog.RuralGovs;

namespace FSH.WebApi.Host.Controllers.Catalog;

public class RuralGovsController: VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search,FSHResource.RuralGovs)]
    [OpenApiOperation("Search rural goverment using available filters.","")]
    public Task<PaginationResponse<RuralGovDto>> SearchAsync(SearchRuralGovsRequest request)
    {
        return Mediator.Send(request);
    }
    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View,FSHResource.RuralGovs)]
    [OpenApiOperation("get rural goverment details.","")]
    public Task<RuralGovDto> GetAsync(DefaultIdType id)
    {
        return Mediator.Send(new GetRuralGovRequest(id));
    }
    [HttpPost]
    [MustHavePermission(FSHAction.Create,FSHResource.RuralGovs)]
    [OpenApiOperation("Create a new rural goverment","")]
    public Task<DefaultIdType> CreateAsync(CreateRuralGovRequest request)
    { return Mediator.Send(request); }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update,FSHResource.RuralGovs)]
    [OpenApiOperation("Update a rural goverment","")]
    public async Task<ActionResult<DefaultIdType>> UpdateAsync(UpdateRuralGovRequest request,DefaultIdType id)
    {
        return id!=request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete,FSHResource.RuralGovs)]
    [OpenApiOperation("Delete a rural goverment","")]
    public Task<DefaultIdType> DeleteAsync(DefaultIdType id)
    {
        return Mediator.Send(new DeleteRuralGovRequest(id));
    }
}
