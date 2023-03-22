using System.ComponentModel;

namespace FSH.WebApi.Application.Catalog.FinSupports;

public interface IFinSupportGeneratorJob : IScopedService
{
    [DisplayName("Generate Random FinSupport example job on Queue notDefault")]
    Task GenerateAsync(int nSeed, CancellationToken cancellationToken);

    [DisplayName("removes all random brands created example job on Queue notDefault")]
    Task CleanAsync(CancellationToken cancellationToken);
}
