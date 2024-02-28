using FSH.WebApi.Application.Catalog.Brands;
using FSH.WebApi.Application.Catalog.Contributions;
using FSH.WebApi.Application.Catalog.Products;
using FSH.WebApi.Application.Common.Models;
using FSH.WebApi.Application.Dashboard;
using FSH.WebApi.Application.HomePage;


namespace FSH.WebApi.Host.Controllers.HomePage;


public class HomePageController : VersionNeutralApiController
{
    [HttpGet]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Get statistics for the dashboard.", "")]
    public async Task<MainPageModelDto> GetAsync()
    {
        await Mediator.Send(new GenerateRandomContributionRequest(10));
        return await Mediator.Send(new GetHomePageRequest());
    }
    
    
    [HttpPost]
    [OpenApiOperation("Update main home page.", "")]
    public Task<MainPageModelDto> PostAsync(UpdateHomePageRequest request)
    {
        return Mediator.Send(request);
    }

    
}
