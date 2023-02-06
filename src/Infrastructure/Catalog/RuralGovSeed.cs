using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Domain.Catalog;
using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Infrastructure.Persistence.Context;
using FSH.WebApi.Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Infrastructure.Catalog;
public class RuralGovSeed : ICustomSeeder
{

    private readonly ISerializerService _serializerService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<RuralGovSeed> _logger;

    public RuralGovSeed(ISerializerService serializerService, ILogger<RuralGovSeed> logger, ApplicationDbContext db)
    {
        _serializerService = serializerService;
        _logger = logger;
        _db = db;
    }


    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (!_db.RuralGovs.Any())
        {
            _logger.LogInformation("Started to Seed Rural goverments.");

            // Here you can use your own logic to populate the database.
            // As an example, I am using a JSON file to populate the database.
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string brandData = await File.ReadAllTextAsync(path + "/Catalog/RuralGovs.json", cancellationToken);
            var ruralGovs = _serializerService.Deserialize<List<RuralGov>>(brandData);

            if (ruralGovs != null)
            {
                foreach (var rutalgov in ruralGovs)
                {
                    await _db.RuralGovs.AddAsync(rutalgov, cancellationToken);
                }
            }

            await _db.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Seeded Rural goverments.");
        }
    }
}
