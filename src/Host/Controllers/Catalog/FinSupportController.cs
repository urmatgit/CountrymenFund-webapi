using FSH.WebApi.Application.Catalog.FinSupports;

namespace FSH.WebApi.Host.Controllers.Catalog;

public class FinSupportsController : VersionedApiController
{
    [HttpPost("search")]
    [MustHavePermission(FSHAction.Search, FSHResource.FinSupport)]
    [OpenApiOperation("Search finSupports using available filters.", "")]
    public Task<PaginationResponse<FinSupportDto>> SearchAsync(SearchFinSupportsRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:guid}")]
    [MustHavePermission(FSHAction.View, FSHResource.FinSupport)]
    [OpenApiOperation("Get finSupport details.", "")]
    public Task<FinSupportDto> GetAsync(Guid id)
    {
        return Mediator.Send(new GetFinSupportRequest(id));
    }

    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.FinSupport)]
    [OpenApiOperation("Create a new finSupport.", "")]
    public Task<Guid> CreateAsync(CreateFinSupportRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.FinSupport)]
    [OpenApiOperation("Update a finSupport.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateFinSupportRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.FinSupport)]
    [OpenApiOperation("Delete a finSupport.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteFinSupportRequest(id));
    }

    [HttpPost("generate-random")]
    [MustHavePermission(FSHAction.Generate, FSHResource.FinSupport)]
    [OpenApiOperation("Generate a number of random finSupports.", "")]
    public Task<string> GenerateRandomAsync(GenerateRandomFinSupportRequest request)
    {
        return Mediator.Send(request);
    }

    //[HttpDelete("delete-random")]
    //[MustHavePermission(FSHAction.Clean, FSHResource.FinSupport)]
    //[OpenApiOperation("Delete the finSupports generated with the generate-random call.", "")]
    //[ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Search))]
    //public Task<string> DeleteRandomAsync()
    //{
    //    return Mediator.Send(new DeleteRandomFinSupportRequest());
    //}
}