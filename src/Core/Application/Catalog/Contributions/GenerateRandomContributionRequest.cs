using FSH.WebApi.Application.Catalog.Brands;
using FSH.WebApi.Domain.Common.Events;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Contributions;
public class GenerateRandomContributionRequest: IRequest<string>
{
    public int IntervalMinute { get; set; }
    public GenerateRandomContributionRequest(int interval)
    {
            IntervalMinute = interval;
    }

}
public class GenerateRandomContribution : IRequest
{
    

}
public class GenerateRandomContributionRequestHandler : IRequestHandler<GenerateRandomContributionRequest, string>
{
    private readonly IJobService _jobService;

    public GenerateRandomContributionRequestHandler(IJobService jobService) => _jobService = jobService;

    public Task<string> Handle(GenerateRandomContributionRequest request, CancellationToken cancellationToken)
    {
        string jobId = "GenerateJob";
        _jobService.AddOrUpdate(jobId,new GenerateRandomContribution(), Cron.MinuteInterval(3));
        return Task.FromResult(jobId);
    }
}


