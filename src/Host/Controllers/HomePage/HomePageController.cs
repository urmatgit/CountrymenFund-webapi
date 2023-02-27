using FSH.WebApi.Application.Dashboard;
using FSH.WebApi.Application.HomePage;
using FSH.WebApi.Shared.Common;

namespace FSH.WebApi.Host.Controllers.HomePage;

public class HomePageController : VersionedApiController
{
    [HttpGet]
    [AllowAnonymous]
    [OpenApiOperation("Get statistics for the dashboard.", "")]
    public Task<MainPageModel> GetAsync()
    {
        return Mediator.Send(new GetHomePageRequest());
    }
}
