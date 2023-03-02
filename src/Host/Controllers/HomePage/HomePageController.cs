using FSH.WebApi.Application.Dashboard;
using FSH.WebApi.Application.HomePage;
using FSH.WebApi.Shared.Common;

namespace FSH.WebApi.Host.Controllers.HomePage;


public class HomePageController : VersionNeutralApiController
{
    [HttpGet]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Get statistics for the dashboard.", "")]
    public Task<MainPageModel> GetAsync()
    {
        return Mediator.Send(new GetHomePageRequest());
    }
    [HttpPost]
    [OpenApiOperation("Update main home page.", "")]
    public Task<MainPageModel> PostAsync(UpdateHomePageRequest request)
    {
        return Mediator.Send(request);
    }

}
