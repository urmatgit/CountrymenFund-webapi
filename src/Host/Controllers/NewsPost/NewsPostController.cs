using FSH.WebApi.Application.HomePage;
using FSH.WebApi.Application.NewsPosts;

namespace FSH.WebApi.Host.Controllers.NewsPost;

public class NewsPostController : VersionNeutralApiController
{
    //News post
    [HttpPost("search")]
    [AllowAnonymous]
    //[MustHavePermission(FSHAction.View, FSHResource.NewsPost)]
    [TenantIdHeader]
    [OpenApiOperation("Search brands using available filters.", "")]
    public Task<PaginationResponse<NewsPostDto>> SearchAsync(SearchNewsPostRequest request)
    {
        return Mediator.Send(request);
    }
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    //[MustHavePermission(FSHAction.View, FSHResource.NewsPost)]
    [TenantIdHeader]
    [OpenApiOperation("Get news post.", "")]
    public   Task<NewsPostDto> GetAsync(Guid id)
    {
        return   Mediator.Send( new GetNewsPostRequest(id));
    }


    [HttpPost]
    [MustHavePermission(FSHAction.Create, FSHResource.NewsPost)]
    [OpenApiOperation("Create a new news post.", "")]
    public Task<Guid> CreateAsync(CreateNewsPostRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:guid}")]
    [MustHavePermission(FSHAction.Update, FSHResource.NewsPost)]
    [OpenApiOperation("Update a newspost.", "")]
    public async Task<ActionResult<Guid>> UpdateAsync(UpdateNewsPostRequest request, Guid id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:guid}")]
    [MustHavePermission(FSHAction.Delete, FSHResource.NewsPost)]
    [OpenApiOperation("Delete a news post.", "")]
    public Task<Guid> DeleteAsync(Guid id)
    {
        return Mediator.Send(new DeleteNewsPostRequest(id));
    }

    //[HttpPost("export")]
    //[MustHavePermission(FSHAction.Export, FSHResource.NewsPost)]
    //[OpenApiOperation("Export a products.", "")]
    //public async Task<FileResult> ExportAsync(ExportProductsRequest filter)
    //{
    //    var result = await Mediator.Send(filter);
    //    return File(result, "application/octet-stream", "ProductExports");
    //}
}
