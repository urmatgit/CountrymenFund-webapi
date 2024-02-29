using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Application.Catalog.Natives;
public interface INativeGeneratorJob: IScopedService
{
    [DisplayName("Generate Random natives example job on Queue notDefault")]
Task GenerateAsync(int nSeed, CancellationToken cancellationToken);

[DisplayName("removes all random natives created example job on Queue notDefault")]
Task CleanAsync(CancellationToken cancellationToken);
}
