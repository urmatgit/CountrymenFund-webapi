using FSH.WebApi.Application.Catalog.RuralGovs;
using FSH.WebApi.Application.Common.Interfaces;
using FSH.WebApi.Application.Common.Persistence;
using FSH.WebApi.Domain.Catalog.Fund;
using FSH.WebApi.Infrastructure.Persistence.Context;
using FSH.WebApi.Infrastructure.Persistence.Initialization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.WebApi.Infrastructure.Catalog;
public class NativeSeed: ICustomSeeder
{
    private readonly ISerializerService _serializerService;
    private readonly ApplicationDbContext _db;
    private readonly IReadRepository<RuralGov> _ruralGovRepo;
    private readonly ILogger<BrandSeeder> _logger;

    public NativeSeed(ISerializerService serializerService, ILogger<BrandSeeder> logger, ApplicationDbContext db,IReadRepository<RuralGov> repository)
    {
        _serializerService = serializerService;
        _logger = logger;
        _db = db;
        _ruralGovRepo = repository;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (!_db.Natives.Any())
        {
            var ruralGov = await _ruralGovRepo.GetBySpecAsync(new RuralGovByNameSpec("Кара-Кулжа"), cancellationToken);
            if (ruralGov == null)
            {
                var native = new Native("Урмат", "Эркимбаев", "", new DateTime(1982, 9, 2),"1 май",  "this is for test",10, ruralGov.Id,"");
                _db.Natives.Add(native);
                await _db.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Seeded native");

            }
        }
    }
}
