using FSH.WebApi.Application.Catalog.Brands;
using FSH.WebApi.Application.Catalog.Contributions;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Domain.Common.Events;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;


public class GenereateRandomNativeRequest : IRequest<string>
{
    public int NSeed { get; set; }
}



public class GenereateRandomNativeRequestHandler : IRequestHandler<GenereateRandomNativeRequest, string>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IJobService _jobService;

    public GenereateRandomNativeRequestHandler(IJobService jobService) => _jobService = jobService;

    public   Task<string> Handle(GenereateRandomNativeRequest request, CancellationToken cancellationToken)
    {
        string jobId = _jobService.Enqueue<INativeGeneratorJob>(x => x.GenerateAsync(request.NSeed, default));
        return Task.FromResult(jobId);
    }
}


