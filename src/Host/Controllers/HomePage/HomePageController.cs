﻿using FSH.WebApi.Application.Catalog.Brands;
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
    public Task<MainPageModelDto> GetAsync()
    {
        return Mediator.Send(new GetHomePageRequest());
    }
    [HttpPost("search")]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Search brands using available filters.", "")]
    public Task<PaginationResponse<NewsPostDto>> SearchAsync(SearchNewsPostRequest request)
    {
        return Mediator.Send(request);
    }
    
    [HttpPost]
    [OpenApiOperation("Update main home page.", "")]
    public Task<MainPageModelDto> PostAsync(UpdateHomePageRequest request)
    {
        return Mediator.Send(request);
    }

}
