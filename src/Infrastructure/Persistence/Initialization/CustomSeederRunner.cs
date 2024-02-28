using FSH.WebApi.Application.Catalog.Contributions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.WebApi.Infrastructure.Persistence.Initialization;

internal class CustomSeederRunner
{
    private readonly ICustomSeeder[] _seeders;
     private readonly ISender _mediator;
    public CustomSeederRunner(IServiceProvider serviceProvider,ISender mediator)
    {
        _seeders = serviceProvider.GetServices<ICustomSeeder>().ToArray();
        _mediator = mediator;
    }

    public async Task RunSeedersAsync(CancellationToken cancellationToken)
    {
        foreach (var seeder in _seeders)
        {
            await seeder.InitializeAsync(cancellationToken);
        }
     
    }
}