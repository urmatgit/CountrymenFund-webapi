namespace FSH.WebApi.Application.Catalog.FinSupports;

public class GenerateRandomFinSupportRequest : IRequest<string>
{
    public int NSeed { get; set; }
}

public class GenerateRandomFinSupportRequestHandler : IRequestHandler<GenerateRandomFinSupportRequest, string>
{
    private readonly IJobService _jobService;

    public GenerateRandomFinSupportRequestHandler(IJobService jobService) => _jobService = jobService;

    public Task<string> Handle(GenerateRandomFinSupportRequest request, CancellationToken cancellationToken)
    {
        string jobId = _jobService.Enqueue<IFinSupportGeneratorJob>(x => x.GenerateAsync(request.NSeed, default));
        return Task.FromResult(jobId);
    }
}